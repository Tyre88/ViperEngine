using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ViperEngine
{
    public class Animation
    {
        Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }

        public int FrameWidth { get; set; }

        public int FrameHeight
        {
            get { return texture.Height; }
        }

        float frameTime;

        public float FrameTime
        {
            get { return frameTime; }
        }

        public int FrameCount { get; set; }

        bool isLooping;

        public bool IsLooping
        {
            get { return isLooping; }
        }


        public Animation(Texture2D texture, int frameWidth, float frameTime, bool isLooping)
        {
            this.texture = texture;
            FrameWidth = frameWidth;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            FrameCount = Texture.Width / FrameWidth;
        }

    }
}
