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
        private Vector2 _centre;
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
            _width = 400;
            _height = 250;
        }


        public Camera(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Update(Vector2 position)
        {
            _centre = new Vector2(position.X - (_width / 2), position.Y - _height);

            //if (_centre.X < _viewport.X)
            //{
            //    _centre.X = _viewport.X;
            //}
            //else if (_centre.X > (_viewport.Width / 2) - (_width))
            //{
            //    _centre.X = _viewport.Width - (_width * 2);
            //}

            //if (_centre.Y < _viewport.Y)
            //{
            //    _centre.Y = _viewport.Y;
            //}
            //else if (_centre.Y > (_viewport.Height / 2) - _height)
            //{
            //    _centre.Y = _viewport.Height - (_height * 2);
            //}

            //_transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
            //             Matrix.CreateTranslation(new Vector3(-_centre.X, -_centre.Y, 0));

            _transform = Matrix.CreateTranslation(new Vector3(-_centre.X, -_centre.Y, 0)) *
                         Matrix.CreateRotationZ(Rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));
        }
    }
}
