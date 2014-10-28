using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ViperEngine.GameObjects
{
    public class Player
    {
        public delegate void PDrawEventHandler(Player player, SpriteBatch spriteBatch);
        public delegate void PUpdateEventHandler(Player player, GameTime gameTime);

        List<Vector2> position;
        public Texture2D head, body, back;
        protected List<Texture2D> bodyList;
        protected System.TimeSpan lastUpdate;
        private const int dimension = 16;
        List<float> rotation;

        private Vector2 baseOrigin = new Vector2(8, 8);

        List<Vector2> origin;

        public event EventHandler Die;
        public event PUpdateEventHandler UpdateEvents;
        protected event PUpdateEventHandler TimeUpdateEvent;
        public event PDrawEventHandler DrawEvents;

        int points;

        Vector2 lastPosition, newPosition, lastOrigin, newOrigin;

        float lastRotation, newRotation;

        public LastKeyState lastkeystate = LastKeyState.DOWN;

        protected int dificulty;

        public static float timer, immortalTimer;
        float animationTimer = 100;
        float immortalDuration = 2500;
        bool normalColor = true;
        public static bool immortal = false;

        private Keys _up, _down, _right, _left;

        public List<Texture2D> BodyList
        {
            get { return bodyList; }
            set { bodyList = value; }
        }

        public float Speed { get; set; }

        public float Percentage { get; set; }

        public float Timer { get; set; }

        public Color Color { get; set; }

        public Effect PlayerEffect { get; set; }

        public bool Alive { get; set; }

        public string Name { get; set; }

        public Keys Up
        {
            get { return _up; }
            internal set { _up = value; }
        }

        public Keys Down
        {
            get { return _down; }
            internal set { _down = value; }
        }

        public Keys Right
        {
            get { return _right; }
            internal set { _right = value; }
        }

        public Keys Left
        {
            get { return _left; }
            internal set { _left = value; }
        }

        public int Points
        {
            get { return points; }
        }

        public Vector2 Position
        {
            get { return position.First(); }
        }

        public List<Vector2> BodyPositions
        {
            get { return position.Skip(1).ToList(); }
        }

        public Player(float X, float Y)
        {
            position = new List<Vector2>();
            bodyList = new List<Texture2D>();
            position.Add(new Vector2(X, Y));
            origin = new List<Vector2>();
            origin.Add(new Vector2(8, 8));

            rotation = new List<float>();
            rotation.Add(0);

            Init(Dificulty.MEDIUM);
        }

        public Player(float X, float Y, Keys up, Keys down, Keys right, Keys left)
        {
            _up = up;
            _down = down;
            _right = right;
            _left = left;

            position = new List<Vector2>();
            bodyList = new List<Texture2D>();
            position.Add(new Vector2(X, Y));

            rotation = new List<float>();
            rotation.Add(0);

            origin = new List<Vector2>();
            origin.Add(new Vector2(8, 8));

            Init(Dificulty.MEDIUM);
        }

        protected void Init(Dificulty dificulty)
        {
            if (dificulty == Dificulty.EASY)
            {
                this.dificulty = 300;
            }
            else if (dificulty == Dificulty.MEDIUM)
            {
                this.dificulty = 150;
            }
            else if (dificulty == Dificulty.HARD)
            {
                this.dificulty = 50;
            }

            Alive = true;
        }

        public void LoadContent(ContentManager Content)
        {
            head = Content.Load<Texture2D>(@"Player/snake_head16");
            body = Content.Load<Texture2D>(@"Player/snake_mid16");
            back = Content.Load<Texture2D>(@"Player/snake_end16");

            for (int i = 0; i < 3; i++)
            {
                bodyList.Add(body);
                position.Add(new Vector2(position[i].X, position[i].Y - 15));
                rotation.Add(0);
                origin.Add(new Vector2(8, 8));
            }
        }

        public void Kill()
        {
            this.Die(this, null);
        }

        public void Reinitialize(float X, float Y, Keys up, Keys down, Keys right, Keys left)
        {
            _up = up;
            _down = down;
            _right = right;
            _left = left;

            position = new List<Vector2>();
            bodyList = new List<Texture2D>();
            position.Add(new Vector2(X, Y));
            origin = new List<Vector2>();
            origin.Add(new Vector2(8, 8));

            rotation = new List<float>();
            rotation.Add(0);

            for (int i = 0; i < 3; i++)
            {
                bodyList.Add(body);
                position.Add(new Vector2(position[i].X, position[i].Y - 15));
                rotation.Add(0);
                origin.Add(new Vector2(8, 8));
            }

            lastkeystate = LastKeyState.DOWN;

            Init(Dificulty.MEDIUM);
        }

        public virtual void Update(GameTime gameTime)
        {
            lastPosition = position[position.Count - 1];
            newPosition = position[0];

            lastRotation = rotation[rotation.Count - 1];
            newRotation = rotation[0];

            lastOrigin = origin[origin.Count - 1];
            newOrigin = origin[0];

            if (lastkeystate == LastKeyState.RIGHT)
            {
                newPosition.X += dimension;
                newRotation = -(float)Math.PI / 2.0f;
            }
            else if (lastkeystate == LastKeyState.LEFT)
            {
                newPosition.X -= dimension;
                newRotation = (float)Math.PI / 2.0f;
            }
            else if (lastkeystate == LastKeyState.DOWN)
            {
                newPosition.Y += dimension;
                newRotation = 0f;
            }
            else if (lastkeystate == LastKeyState.UP)
            {
                newPosition.Y -= dimension;
                newRotation = (float)Math.PI;
            }

            if ((lastUpdate.TotalMilliseconds + dificulty) - Speed <= gameTime.TotalGameTime.TotalMilliseconds)
            {
                rotation[0] = MathHelper.Lerp(rotation[0], newRotation, 0.5f);

                for (int i = 0; i < position.Count; i++)
                {
                    lastPosition = position[i];
                    position[i] = newPosition;
                    newPosition = lastPosition;

                    lastRotation = rotation[i];
                    rotation[i] = newRotation;

                    lastOrigin = origin[i];
                    origin[i] = newOrigin;

                    newRotation = lastRotation;
                    newOrigin = lastOrigin;
                }

                lastUpdate = gameTime.TotalGameTime;

                if (TimeUpdateEvent != null)
                {
                    TimeUpdateEvent(this, gameTime);
                }
            }

            if (UpdateEvents != null)
            {
                UpdateEvents(this, gameTime);
            }
        }

        public void Addpoints(int points)
        {
            bodyList.Add(body);
            position.Add(lastPosition);
            rotation.Add(lastRotation);
            origin.Add(new Vector2(8, 8));
            this.points += points;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (immortal)
            {
                timer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
                immortalTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;
                if (timer > animationTimer)
                {
                    if (normalColor)
                    {
                        normalColor = false;
                    }
                    else if (!normalColor)
                    {
                        normalColor = true;
                    }
                    if (immortalTimer > immortalDuration)
                    {
                        immortal = false;
                        normalColor = true;
                        immortalTimer = 0;
                    }
                    timer = 0;
                }
            }
            if (normalColor)
            {
                for (int i = 0; i < bodyList.Count; i++)
                {
                    //spriteBatch.Draw(bodyList[i], position[i + 1], null, Color.White, rotation[i], Vector2.Zero, 0f, SpriteEffects.None, 0f);
                    if (i == bodyList.Count - 1)
                    {
                        spriteBatch.Draw(back, position[i + 1], null, Color.White, rotation[i + 1], origin[i + 1], 1f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.Draw(bodyList[i], position[i + 1], null, Color.White, rotation[i + 1], origin[i + 1], 1f, SpriteEffects.None, 1f);
                    }
                }
            }
            else if (!normalColor)
            {
                for (int i = 0; i < bodyList.Count; i++)
                {
                    spriteBatch.Draw(bodyList[i], position[i + 1], Color.LightGray);
                }
            }
            //spriteBatch.Draw(head, position[0], Color.White);
            spriteBatch.Draw(head, position[0], null, Color.White, rotation[0], origin[0], 1f, SpriteEffects.None, 1f);

            if (DrawEvents != null)
            {
                DrawEvents(this, spriteBatch);
            }
        }
    }
}
