using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ViperEngine.GameObjects
{
    public class GameObject
    {
        public delegate void GOEventHandler(Player player);

        private Texture2D _texture;
        private Vector2 _position, _origin;
        private bool _evil;
        private int _width, _height;
        Random _random;
        private List<Texture2D> _textureList;
        private int _currentTexture = 0;
        private float _timer;
        private float _animationTimer = 300;
        public Rectangle sheetItem;

        protected const int objectWidth = 16;
        protected const int objectHeight = 16;

        public event GOEventHandler Eat;

        public int Score { get; set; }


        public Rectangle SheetItem
        {
            get { return sheetItem; }
        }

        public bool Evil
        {
            get { return _evil; }
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public GameObject(Texture2D texture, bool evil, int mapWidth, int mapHeight)
        {
            _texture = texture;
            _evil = evil;
            _width = mapWidth / 16;
            _height = mapHeight / 16;
            _random = new Random();
            _position = new Vector2((_random.Next(0, _width) * 16), (_random.Next(0, _height) * 16));
            _origin = new Vector2(8, 8);
        }

        public GameObject(Texture2D texture, bool evil, int mapWidth, int mapHeight, Rectangle sheetItem)
        {
            _texture = texture;
            _evil = evil;
            _width = mapWidth / 16;
            _height = mapHeight / 16;
            _random = new Random();
            _position = new Vector2((_random.Next(0, _width) * 16), (_random.Next(0, _height) * 16));
            this.sheetItem = sheetItem;
            _origin = new Vector2(8, 8);
        }

        public GameObject(List<Texture2D> textureList, bool evil, int mapWidth, int mapHeight)
        {
            _textureList = textureList;
            _texture = _textureList[0];
            _evil = evil;
            _width = mapWidth / 16;
            _height = mapHeight / 16;
            _random = new Random();
            _position = new Vector2((_random.Next(0, _width) * 16), (_random.Next(0, _height) * 16));
            _origin = new Vector2(8, 8);
        }

        public GameObject(List<Texture2D> textureList, bool evil, int mapWidth, int mapHeight, Rectangle sheetItem)
        {
            _textureList = textureList;
            _texture = _textureList[0];
            _evil = evil;
            _width = mapWidth / 16;
            _height = mapHeight / 16;
            _random = new Random();
            _position = new Vector2((_random.Next(0, _width) * 16), (_random.Next(0, _height) * 16));
            this.sheetItem = sheetItem;
            _origin = new Vector2(8, 8);
        }

        public void EatObject(Player player)
        {
            player.Addpoints(Score);
            if (Eat != null)
            {
                this.Eat(player);
            }
        }

        public void Reinitialize()
        {
            _random = new Random();
            _position = new Vector2((_random.Next(0, _width) * 16), (_random.Next(0, _height) * 16));
        }

        protected void Dispose()
        {
            Engine.Singleton.GameObjects.Remove(this);
        }

        public virtual void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;

            if (_textureList != null)
            {
                if (_currentTexture < _textureList.Count - 1 && _timer > _animationTimer)
                {
                    _currentTexture++;
                    _texture = _textureList[_currentTexture];
                    _timer = 0;
                }
                else if (_timer > _animationTimer)
                {
                    _currentTexture = 0;
                    _texture = _textureList[_currentTexture];
                    _timer = 0;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture, _position, sheetItem, Color.White, 0, _origin, 1f, SpriteEffects.None, 0);
        }
    }
}
