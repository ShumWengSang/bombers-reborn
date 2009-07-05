using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectDraw;

namespace MineBomber_Engine
{
    public class Sprite
    {
        public enum Direction
        {
            Right,
            Left,
            Top,
            Bottom,
            None
        }

        public Sprite()
        {
            
        }

        public int NSurface
        {
            get { return _nSurface; }
            set { _nSurface = value; }
        }

        public Point MapPoint
        {
            get { return _mapPoint; }
            set { _mapPoint = value; }
        }

        public long Tick
        {
            get { return _tick; }
            set { _tick = value; }
        }

        public Point Location
        {
            get {
                return new Point(MineBomberEngine.CELL_SIZE*_mapPoint.X,
                                 MineBomberEngine.CELL_SIZE*_mapPoint.Y);
            }
            set { _mapPoint = new Point(value.X/MineBomberEngine.CELL_SIZE, value.Y/MineBomberEngine.CELL_SIZE); }
        }

        public int Faze
        {
            get { return _faze; }
            set { _faze = value; }
        }

        public long Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public MineBomberEngine FEngine
        {
            get { return _fEngine; }
            set { _fEngine = value; }
        }

        public Direction MoveDirection
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public virtual void Draw()
        {
            try
            {
                _fEngine.BackSurface.Draw(_dRect, _fEngine.SpriteSurface[_nSurface], _aRect, 0);
            }
            catch (InvalidRectangleException)
            {
                Trace.TraceError("Type: {0}\n aRect: {1}; dRect {2}", this.ToString(), _aRect.ToString(), _dRect.ToString());
                throw;
            }
        }

        protected Rectangle _aRect;
        protected Rectangle _dRect;
        protected MineBomberEngine _fEngine;
        protected Point _point;
        protected Point _mapPoint;
        protected int _faze;
        protected int _stype;
        protected int _width;
        protected int _height;
        protected int _numParent;
        protected long _tick;
        protected long _speed;
        protected int _nSurface;
        private Direction _direction;
    }
}
