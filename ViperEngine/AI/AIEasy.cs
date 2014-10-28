using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViperEngine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ViperEngine.AI
{
    public class AIEasy : Player
    {
        private const int MAXTileSight = 5;

        private int _tileSight;

        Random _rnd;

        private bool HasCollideRight(int x)
        {
            return Engine.Singleton.currentMap.collisionRects.FindAll(c => c.Intersects(new Rectangle((int)(base.Position.X + (int)(x * Engine.Singleton.currentMap.TileWidth)), (int)base.Position.Y,
                Engine.Singleton.currentMap.MapWidth, Engine.Singleton.currentMap.MapHeight))).Count > 0;
        }

        private bool HasCollideLeft(int x)
        {
            return Engine.Singleton.currentMap.collisionRects.FindAll(c => c.Intersects(new Rectangle((int)(base.Position.X - (int)(x * Engine.Singleton.currentMap.TileWidth)), (int)base.Position.Y,
                Engine.Singleton.currentMap.MapWidth, Engine.Singleton.currentMap.MapHeight))).Count > 0;
        }

        private bool HasCollideBottom(int y)
        {
            return Engine.Singleton.currentMap.collisionRects.FindAll(c => c.Intersects(new Rectangle((int)base.Position.X, (int)(base.Position.Y + (int)(y * Engine.Singleton.currentMap.TileHeight)),
                Engine.Singleton.currentMap.MapWidth, Engine.Singleton.currentMap.MapHeight))).Count > 0;
        }

        private bool HasCollideTop(int y)
        {
            return Engine.Singleton.currentMap.collisionRects.FindAll(c => c.Intersects(new Rectangle((int)base.Position.X, (int)(base.Position.Y - (int)(y * Engine.Singleton.currentMap.TileHeight)),
                Engine.Singleton.currentMap.MapWidth, Engine.Singleton.currentMap.MapHeight))).Count > 0;
        }

        private bool CollideSelf(LastKeyState nextKeyState)
        {
            switch (nextKeyState)
            {
                    case LastKeyState.DOWN:
                    return base.BodyPositions.FindAll(MatchDown).Count > 0;
                    break;
                    case LastKeyState.UP:
                    return base.BodyPositions.FindAll(this.MatchUp).Count > 0;
                    break;
                    case LastKeyState.RIGHT:
                    return base.BodyPositions.FindAll(this.MatchRight).Count > 0;
                    break;
                    case LastKeyState.LEFT:
                    return base.BodyPositions.FindAll(this.MatchLeft).Count > 0;
                    break;
            }

            return false;
        }

        private bool MatchDown(Vector2 vector2)
        {
            Rectangle rect = new Rectangle((int)vector2.X, (int)vector2.Y, base.BodyList.First().Width, base.BodyList.First().Height);

            return rect.Intersects(new Rectangle((int)Position.X, ((int)Position.Y + head.Height), head.Width, head.Height));
        }

        private bool MatchUp(Vector2 vector2)
        {
            Rectangle rect = new Rectangle((int)vector2.X, (int)vector2.Y, base.BodyList.First().Width, base.BodyList.First().Height);

            return rect.Intersects(new Rectangle((int)Position.X, ((int)Position.Y - head.Height), head.Width, head.Height));
        }

        private bool MatchRight(Vector2 vector2)
        {
            Rectangle rect = new Rectangle((int)vector2.X, (int)vector2.Y, base.BodyList.First().Width, base.BodyList.First().Height);

            return rect.Intersects(new Rectangle(((int)Position.X + head.Width), (int)Position.Y, head.Width, head.Height));
        }

        private bool MatchLeft(Vector2 vector2)
        {
            Rectangle rect = new Rectangle((int)vector2.X, (int)vector2.Y, base.BodyList.First().Width, base.BodyList.First().Height);

            return rect.Intersects(new Rectangle(((int)Position.X - head.Width), (int)Position.Y, head.Width, head.Height));
        }

        public AIEasy(float X, float Y)
            :base(X, Y)
        {
            base.Init(Dificulty.MEDIUM);

            lastkeystate = LastKeyState.RIGHT;

            TimeUpdateEvent += Detection;

            _rnd = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        void Detection(Player player, GameTime gameTime)
        {
            _tileSight = _rnd.Next(MAXTileSight);

            for (int i = 1; i <= _tileSight; i++)
            {
                CollisionDetection(i);
            }
        }

        private void CollisionDetection(int sight)
        {
            if (lastkeystate == LastKeyState.RIGHT || lastkeystate == LastKeyState.LEFT)
            {
                if(HasCollideRight(sight) || this.HasCollideLeft(sight))
                {
                    for (int i = 0; i < sight; i++)
			        {
                        if(HasCollideTop(i))
                        {
                            if (!this.CollideSelf(LastKeyState.DOWN))
                            {
                                lastkeystate = LastKeyState.DOWN;
                            }
                        }
                        else if(HasCollideBottom(i))
                        {
                            if (!this.CollideSelf(LastKeyState.UP))
                            {
                                lastkeystate = LastKeyState.UP;
                            }
                        }
                        else
                        {
                            lastkeystate = _rnd.Next(2) > 0 ? LastKeyState.UP : LastKeyState.DOWN;
                        }
			        }
                }
            }
            else if (lastkeystate == LastKeyState.UP || lastkeystate == LastKeyState.DOWN)
            {
                if (this.HasCollideTop(sight) || this.HasCollideBottom(sight))
                {
                    for (int i = 0; i < sight; i++)
                    {
                        if (this.HasCollideRight(i))
                        {
                            if (!this.CollideSelf(LastKeyState.LEFT))
                            {
                                lastkeystate = LastKeyState.LEFT;
                            }
                        }
                        else if (this.HasCollideLeft(i))
                        {
                            if (!this.CollideSelf(LastKeyState.RIGHT))
                            {
                                lastkeystate = LastKeyState.RIGHT;
                            }
                        }
                        else
                        {
                            lastkeystate = _rnd.Next(2) > 0 ? LastKeyState.LEFT : LastKeyState.RIGHT;
                        }
                    }
                }
            }
        }
    }
}
