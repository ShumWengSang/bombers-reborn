using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MineBomber_Engine
{
    public class Button : Sprite
    {
        private List<Point> _doors = new List<Point>(2);

        public List<Point> Doors
        {
            get { return _doors; }
        }
    }
}
