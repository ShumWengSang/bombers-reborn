using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MineBomber_Engine
{
    public class MySprite : Sprite
    {
        public MySprite(
            MineBomberEngine fEngine,
            Point mapPoint,
            int width,
            int height,
            int stype,
            int nSurface) : base()
        {
            FEngine = fEngine;
            MapPoint = mapPoint;
            Location = new Point(
                MineBomberEngine.CELL_SIZE * mapPoint.X, 
                MineBomberEngine.CELL_SIZE * mapPoint.Y);
            _width = width;
            _height = height;
            _stype = stype;
            NSurface = nSurface;
            Stopped = true;
        }

        public bool Stopped { get; set; }
        public bool Killed { get; set; }
        public bool Mined { get; set; }

        public int MineSpeed
        {
            get { return _mineSpeed; }
        }

        public int Weapon
        {
            get { return _weapon; }
        }

        public int Live
        {
            get { return _live; }
            set { _live = value; }
        }

        public int Money
        {
            get { return _money; }
            set { _money = value; }
        }

        private int _mineSpeed;
        private int _weapon;
        private int _live;
        private int _money;
        private long _mineTick;

        public override void Draw()
        {
            int left = (int) MoveDirection*6*MineBomberEngine.CELL_SIZE + Faze*MineBomberEngine.CELL_SIZE;
            int top = (Mined)
                          ? (_stype + 1)*MineBomberEngine.CELL_SIZE
                          : (_stype + 5)*MineBomberEngine.CELL_SIZE;

            _aRect = new Rectangle(
                left,
                top,
                _width,
                _height);
            
            _dRect = new Rectangle(
                Location.X,
                Location.Y + MineBomberEngine.MAP_TOP_BORDER_OFFSET,
                MineBomberEngine.CELL_SIZE,
                MineBomberEngine.CELL_SIZE);

            base.Draw();
        }

        public void Move()
        {
            int mapX = _mapPoint.X;
            int mapY = _mapPoint.Y;

            switch (FEngine.GameMap.MapChars[mapX][mapY])
            {
                case 'Џ': 
                case 'э': 
                case 'ђ': 
                case '‘': 
                case 'y': 
                case 'm': 
                case '’': 
                case '“': 
                case '”': 
                case '•': 
                case '–': 
                case '—': 
                case '': 
                case '™': 
                case 'љ':
                    switch (FEngine.GameMap.MapChars[mapX][mapY])
                    {
                        case 'm':
                            _live = 100;
                            break;
                        case '‘': 
                            _money += 1;
                            break;
                        case '’':
                            _money += 2;
                            break;
                        case '“':
                            _money += 5;
                            break;
                        case '”':
                            _money += 10;
                            break;
                        case '•':
                            _money += 20;
                            break;
                        case '–':
                            _money += 50;
                            break;
                        case '—':
                            _money += 100;
                            break;
                        case '':
                            _money += 300;
                            break;
                        case '™':
                            _money += 1000;
                            break;
                    }
                    FEngine.GameMap.MapChars[mapX][mapY] = '0';
                    FEngine.GameMap.Draw();
                    break;
            }

            if(Tick + Speed < FEngine.Tick)
            {
                if(Location.X != mapX * MineBomberEngine.CELL_SIZE ||
                    Location.Y != mapY * MineBomberEngine.CELL_SIZE)
                {
                    if (Location.X < mapX * MineBomberEngine.CELL_SIZE) { _point.X++; }
                    if (Location.Y < mapY * MineBomberEngine.CELL_SIZE) { _point.Y++; }
                    if (Location.X > mapX * MineBomberEngine.CELL_SIZE) { _point.X--; }
                    if (Location.Y > mapY * MineBomberEngine.CELL_SIZE) { _point.Y--; }
                }
                if (Faze < 3)
                {
                    Faze++;
                }
                else
                {
                    if(!Stopped)
                    {
                        switch(MoveDirection)
                        {
                            case Direction.Left:
                                if(IsValid(mapX, mapY))
                                {
                                    --mapX;
                                }
                                break;
                            case Direction.Right:
                                if (IsValid(mapX, mapY))
                                {
                                    ++mapX;
                                }
                                break;
                            case Direction.Top:
                                if (IsValid(mapX, mapY))
                                {
                                    --mapY;
                                }
                                break;
                            case Direction.Bottom:
                                if (IsValid(mapX, mapY))
                                {
                                    ++mapY;
                                }
                                break;
                        }

                        if(Mined)
                        {
                            if(Faze < 3)
                            {
                                Faze++;
                            }
                            else
                            {
                                Faze = 0;
                            }
                        }
                        _mapPoint.X = mapX;
                        _mapPoint.Y = mapY;
                    }
                }
            }
            FEngine.GameMap.Draw();
        }

        public void Go()
        {
            Stopped = false;
        }

        public void Stop()
        {
            Stopped = true;
        }

        public void Teleport()
        {
            int teleport;
            int dx;
            int dy;

            do
            {
                teleport = new Random().Next(FEngine.GameMap.Teleports.Count);
            } while (FEngine.GameMap.Teleports[teleport].X != _mapPoint.X ||
                     FEngine.GameMap.Teleports[teleport].Y != MapPoint.Y);

            do
            {
                dx = 0;
                dy = 0;
                MoveDirection = (Direction) new Random().Next(4);

                switch (MoveDirection)
                {
                    case Direction.Left:
                        dx = -1;
                        break;
                    case Direction.Right:
                        dx = 1;
                        break;
                    case Direction.Top:
                        dy = -1;
                        break;
                    case Direction.Bottom:
                        dy = 1;
                        break;
                }
            } while (
                IsValid(FEngine.GameMap.Teleports[teleport].X + dx,
                        FEngine.GameMap.Teleports[teleport].Y + dy));

            _mapPoint.X = FEngine.GameMap.Teleports[teleport].X + dx;
            _mapPoint.Y = FEngine.GameMap.Teleports[teleport].Y + dy;

            Location = new Point(
                _mapPoint.X*MineBomberEngine.CELL_SIZE,
                _mapPoint.Y*MineBomberEngine.CELL_SIZE);
        }

        public void PushButton(int mapX, int mapY)
        {
            Button pushedButton = null;

            foreach (var button in FEngine.GameMap.Buttons)
            {
                if(button.MapPoint.X == mapX && button.MapPoint.Y == mapY)
                {
                    pushedButton = button;
                    break;
                }
            }

            if (pushedButton != null)
                foreach (var doorLocation in pushedButton.Doors)
                {
                    FEngine.GameMap.MapChars[doorLocation.X][doorLocation.Y] =
                        (FEngine.GameMap.MapChars[doorLocation.X][doorLocation.Y] == '1')
                            ? '0'
                            : '1';

                    pushedButton.Tick = FEngine.Tick;
                    Draw();
                }
        }

        public void PutBomb()
        {
            var newBomb = new Bomb()
                              {
                                  Location = MapPoint,
                                  Faze = 0,
                                  Tick = FEngine.Tick,
                                  Speed = 500,
                                  FEngine = FEngine
                              };

            switch (Weapon)
            {
                case 0:
                    newBomb.Power = 2;
                    break;
                case 1:
                    newBomb.Power = 5;
                    break;
                case 2:
                    newBomb.Power = 9;
                    break;
            }

            FEngine.Bombs.Add(newBomb);
        }
        
        public void NextWeapon()
        {
            if(_weapon < 2)
            {
                _weapon++;
            }
            else
            {
                _weapon = 0;
            }
        }

        public void DrawTitle()
        {
            var dRect = new Rectangle(
                0,
                0,
                MineBomberEngine.SCREEN_WIDTH,
                MineBomberEngine.MAP_TOP_BORDER_OFFSET);

            FEngine.BackSurface.DrawFast(0, 0, FEngine.SpriteSurface[3], dRect, 0);
        }

        public void DrawLife()
        {
            var dc = FEngine.BackSurface.GetDc();
            try
            {
                Graphics dxGraphics = Graphics.FromHdc(dc);
                dxGraphics.DrawString(
                    Money.ToString(),
                    new Font("Arial", 18),
                    Brushes.Blue,
                    65 + _numParent * 189,
                    31f);
            }
            finally
            {
                FEngine.BackSurface.ReleaseDc(dc);
            }
        }

        public void DrawWeapon()
        {
            var aRect = new Rectangle(
                5*MineBomberEngine.CELL_SIZE,
                (_weapon + 2)*MineBomberEngine.CELL_SIZE,
                MineBomberEngine.CELL_SIZE,
                MineBomberEngine.CELL_SIZE);

            FEngine.BackSurface.DrawFast(0, 0, FEngine.SpriteSurface[1], aRect, 0);
        }

        public bool IsValid(int mapX, int mapY)
        {
            bool result = false;
            long currentTick = FEngine.Tick;

            switch (FEngine.GameMap.MapChars[mapX][mapY])
            {
                case 'њ': 
                case 'Џ': 
                case 'э': 
                case 'ђ': 
                case '‘': 
                case 'y': 
                case 'm': 
                case '¬': 
                case 'M': 
                case 'N': 
                case 'K': 
                case 'L': 
                case '’': 
                case '“': 
                case '”': 
                case '•': 
                case '–': 
                case '—': 
                case '': 
                case '™': 
                case 'љ':
                    Speed = 0;
                    result = true;
                    break;
                case '2':
                case '3':
                case '4':
                    FEngine.GameMap.MapChars[mapX][mapY] = '0';
                    FEngine.GameMap.Draw();
                    Tick = currentTick;
                    Speed = 10;
                    break;
                case '5':
                    FEngine.GameMap.MapChars[mapX][mapY] = '2';
                    FEngine.GameMap.Draw();
                    Tick = currentTick;
                    Speed = 20;
                    break;
                default:
                    if (FEngine.GameMap.MapChars[mapX][mapY] > '6' &&
                        FEngine.GameMap.MapChars[mapX][mapY] < 'A')
                    {
                        FEngine.GameMap.MapChars[mapX][mapY] = '5';
                        FEngine.GameMap.Draw();
                        Tick = currentTick;
                    }

                    if (FEngine.GameMap.MapChars[mapX][mapY] > 'C' &&
                        FEngine.GameMap.MapChars[mapX][mapY] < 'F')
                    {
                        if(Mined)
                        {
                            if(_mineTick + _mineSpeed < currentTick)
                            {
                                if(FEngine.GameMap.ModMap[mapX][mapY] < 2)
                                {
                                    FEngine.GameMap.ModMap[mapX][mapY]++;
                                }
                                else
                                {
                                    FEngine.GameMap.MapChars[mapX][mapY] = '0';
                                    FEngine.GameMap.ModMap[mapX][mapY] = 0;
                                }

                                FEngine.GameMap.Draw();
                                _mineTick = currentTick;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    break;
            }

            if (FEngine.GameMap.MapChars[mapX][mapY] > 'C' &&
                       FEngine.GameMap.MapChars[mapX][mapY] < 'F')
            {
                Mined = true;
                _mineTick = currentTick + 10;
            }
            else
            {
                Mined = false;
            }

            if(FEngine.GameMap.MapChars[mapX][mapY] == 'ґ')
            {
                PushButton(mapX, mapY);
            }

            return result;
        }
    }
}
