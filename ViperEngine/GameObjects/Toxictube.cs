using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ViperEngine.GameObjects
{
    class Toxictube : GameObject
    {
        Animation toxicAnimation;
        AnimationPlayer animationPlayer;

        public Toxictube(Texture2D texture, int mapWidth, int mapHeight)
            : base(texture, false, mapWidth, mapHeight, new Rectangle(0 * objectWidth, 0 * objectHeight, objectWidth, objectHeight))
        {
            Score = -50;
            animationPlayer = new AnimationPlayer();
            toxicAnimation = new Animation(texture, objectWidth, 0.15f, true);
        }

        public override void Update(GameTime gameTime)
        {
            animationPlayer.PlayAnimation(toxicAnimation);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            animationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None);
        }
    }
}
