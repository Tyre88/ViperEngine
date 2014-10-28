using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ViperEngine.GameObjects
{
    class BrokenBottle : GameObject
    {
        float _timer, _aliveTimer;
        const int _minTimer = 15;
        const int _maxTimer = 60;

        Random random;

        public BrokenBottle(Texture2D texture, int mapWidth, int mapHeight)
            : base(texture, false, mapWidth, mapHeight, new Rectangle(9 * objectWidth, 6 * objectHeight, objectWidth, objectHeight))
        {
            Score = -10;
            random = new Random();
            _aliveTimer = random.Next(_minTimer, _maxTimer) * 1000;
        }

        public BrokenBottle(List<Texture2D> textureList, int mapWidth, int mapHeight)
            : base(textureList, false, mapWidth, mapHeight, new Rectangle(9 * objectWidth, 6 * objectHeight, objectWidth, objectHeight))
        {
            Score = -10;
            random = new Random();
            _aliveTimer = random.Next(_minTimer, _maxTimer) * 1000;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.Milliseconds / 2;

            if (_timer > _aliveTimer)
            {
                base.Dispose();
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
