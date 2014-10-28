using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ViperEngine.GameObjects
{
    class Banana : GameObject
    {
        float _timer, _aliveTimer;

        Random random;

        public Banana(Texture2D texture, int mapWidth, int mapHeight)
            :base(texture, false, mapWidth, mapHeight, new Rectangle(1 * objectWidth, 1 * objectHeight, objectWidth, objectHeight))
        {
            Score = 35;
            random = new Random();
            _aliveTimer = random.Next(5, 15) * 1000;
        }

        public Banana(List<Texture2D> textureList, int mapWidth, int mapHeight)
            : base(textureList, false, mapWidth, mapHeight, new Rectangle(1 * objectWidth, 1 * objectHeight, objectWidth, objectHeight))
        {
            Score = 35;
            random = new Random();
            _aliveTimer = random.Next(5, 15) * 1000;
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
