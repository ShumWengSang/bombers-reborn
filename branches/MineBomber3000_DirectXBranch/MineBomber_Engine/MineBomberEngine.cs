using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectDraw;

namespace MineBomber_Engine
{
    public class MineBomberEngine
    {
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 600;
        public const int CELL_SIZE = 12;
        public const int MAP_WIDTH_CELLS = 64;
        public const int MAP_HEIGHT_CELLS = 45;
        public const int MAP_TOP_BORDER_OFFSET = 50;
        public const int BIT_DEPTH = 16;

        public MineBomberEngine(Control parentControl)
        {
            _parentControl = parentControl;
        }

        public char f;

        private DateTime _systemTime;
        private long _tick;
        private int _fps2;
        private int _fps;
        private Control _parentControl;
        private string[] _spriteBitmaps = new string[100];
        private MySprite[] _mySprites = new MySprite[2];
        private int _numSprites;
        private Map _map;
        private Rectangle _mapRect;
        private Rectangle _renderRect;
        private Surface _mapSurface;
        private Surface _backSurface;
        private Surface _fPrimarySurface;
        private Surface[] _spriteSurface = new Surface[4];
        private Device _fDirectDraw;
        private List<Bomb> _bombs = new List<Bomb>();

        public Surface BackSurface
        {
            get { return _backSurface; }
        }

        public Surface[] SpriteSurface
        {
            get { return _spriteSurface; }
        }

        public Map GameMap
        {
            get { return _map; }
        }

        public MySprite[] MySprites
        {
            get { return _mySprites; }
        }

        public List<Bomb> Bombs
        {
            get { return _bombs; }
        }

        public long Tick
        {
            get { return _tick; }
        }

        public Surface MapSurface
        {
            get { return _mapSurface; }
            set { _mapSurface = value; }
        }

        /// <summary>
        /// Inits DirectDraw and map.
        /// </summary>
        public void InitDraw()
        {
            _map = new Map(this, MAP_WIDTH_CELLS, MAP_HEIGHT_CELLS);
            var clipper = new Clipper {Window = _parentControl};
            _renderRect = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
            _mapRect = new Rectangle(
                0, 
                MAP_TOP_BORDER_OFFSET, 
                MAP_WIDTH_CELLS * CELL_SIZE, 
                MAP_HEIGHT_CELLS * CELL_SIZE);

            _fDirectDraw = new Device();
            _fDirectDraw.SetCooperativeLevel(_parentControl, CooperativeLevelFlags.FullscreenExclusive);
//            _fDirectDraw.SetDisplayMode(SCREEN_WIDTH, SCREEN_HEIGHT, BIT_DEPTH, 0, false);

            var ddPrimarySurfaceDesc = new SurfaceDescription
                                           {
                                               SurfaceCaps = new SurfaceCaps
                                                                 {
                                                                     PrimarySurface = true,
                                                                     VideoMemory = true
                                                                 }
                                           };

            _fPrimarySurface = new Surface(ddPrimarySurfaceDesc, _fDirectDraw)
                                   {
                                       Clipper = clipper
                                   };

            
            var ddBackSurfaceDesc = new SurfaceDescription
                                    {
                                        SurfaceCaps =
                                            {
                                                OffScreenPlain = true,
                                                VideoMemory = true
                                            },
                                        Width = _renderRect.Width,
                                        Height = _renderRect.Height
                                    };
            
            _backSurface = new Surface(ddBackSurfaceDesc, _fDirectDraw);

            var ddSurfaceDesc2 = new SurfaceDescription
            {
                SurfaceCaps =
                {
                    OffScreenPlain = true,
                    VideoMemory = true
                },
                Width = MAP_WIDTH_CELLS * CELL_SIZE,
                Height = MAP_HEIGHT_CELLS * CELL_SIZE
            };
            MapSurface = new Surface(ddSurfaceDesc2, _fDirectDraw);

            BackSurface.FillColor = Color.Black;
        }

        public void InitBass( ) {}
        public void CreateSurfaceFromBitmapN(string path, int number)
        {
            _spriteBitmaps[number] = path;
            _spriteSurface[number] = DDUtils.LoadBitmap(_fDirectDraw, path, 0, 0);
        }

        public void SurfaceSetColorKeyN(Color rgb, int n) {}
        public void ColorFill(Color color) {}
        
        public void Render()
        {
            if(!_parentControl.Created) {return;}

            DrawTitle();
            _systemTime = DateTime.Now;
            _tick = _systemTime.Ticks;
            BackSurface.Draw(_mapRect, MapSurface, 0);

            foreach (var bomb in Bombs)
            {
                bomb.Draw();
            }

            foreach (var mySprite in MySprites)
            {
                mySprite.Move();
                mySprite.Draw();
                mySprite.DrawWeapon();
                mySprite.DrawLife();

                if(mySprite.Live < 0)
                {
                    mySprite.Killed = true;
                    GameMap.MapChars[mySprite.MapPoint.X][mySprite.MapPoint.Y] = 'T';
                    GameMap.Draw();
                }
            }

            DrawFps();

            _fPrimarySurface.Draw(
                new Rectangle(
                    Point.Empty,
                    new Size(SCREEN_WIDTH, SCREEN_HEIGHT)),
                _backSurface,
                DrawFlags.DoNotWait);
        }
        
        public void DrawFps() {}
        public void DrawTitle() {}
        public void DxCheck(int result, string st) {}
        
            
        public void Exit()
        {
            _fDirectDraw.RestoreDisplayMode();
        }

        private delegate Point PointToParent(Control client);
        private Point PointToClientLeftUpperCorner(Control client)
        {
            return client.PointToClient(new Point(0, 0));
        }

      //  public
      //  m:hmusic;
      
      //DOORSWITCHON,SHELLEXPLODE,TELEPORT,TREASURE:HSAMPLE;

    }
}
