using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ViperEngine.Camera
{
    public class Camera
    {
        public Matrix _transform;
        private Vector2 _centre, _oldCentre;
        private int _width, _height;

        private Viewport _viewport;

        private float _zoom = 1;
        private float _rotation = 0;

        public float X
        {
            get { return _centre.X; }
            set { _centre.X = value; }
        }

        public float Y
        {
            get { return _centre.Y; }
            set { _centre.Y = value; }
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            _width = viewport.Width;
            _height = viewport.Height;
        }


        public Camera(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Update(Vector2 position)
        {

            if (Engine.Singleton.numberOfPlayers == Engine.NumberOfPlayers.ONE)
            {
                if ((position.X - (_width)) > 0 && (position.X + (_width)) <= Engine.Singleton.currentMap.MapWidth
                    && (position.Y - (_height)) > 0 && (position.Y + (_height)) <= Engine.Singleton.currentMap.MapHeight)
                {
                    _centre = new Vector2(position.X - _width, position.Y - _height);
                }
                else if ((position.X - (_width)) > 0 && (position.X + (_width)) <= Engine.Singleton.currentMap.MapWidth)
                {
                    _centre = new Vector2(position.X - _width, _oldCentre.Y);
                }
                else if ((position.Y - (_height)) > 0 && (position.Y + (_height)) <= Engine.Singleton.currentMap.MapHeight)
                {
                    _centre = new Vector2(_oldCentre.X, position.Y - _height);
                }
            }
            else
            {
                _centre = new Vector2(position.X - (_width / 2), position.Y - _height);
            }
            
            _transform = Matrix.CreateTranslation(new Vector3(-_centre.X, -_centre.Y, 0)) *
                         Matrix.CreateRotationZ(Rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));

            _oldCentre = _centre;
        }
    }
}
