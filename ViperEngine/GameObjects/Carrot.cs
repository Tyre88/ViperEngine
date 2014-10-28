using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ViperEngine.GameObjects
{
    class Carrot : GameObject
    {
        public Carrot(Texture2D texture, int mapWidth, int mapHeight)
            : base(texture, false, mapWidth, mapHeight, new Rectangle(0 * objectWidth, 0 * objectHeight, objectWidth, objectHeight))
        {
            Score = 25;
        }

        public Carrot(List<Texture2D> textureList, int mapWidth, int mapHeight)
            : base(textureList, false, mapWidth, mapHeight, new Rectangle(0 * objectWidth, 0 * objectHeight, objectWidth, objectHeight))
        {
            Score = 25;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
