using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MineBomber_Engine;

namespace MineBomber_WinFormDisplay
{
    public partial class Form1 : Form
    {
        private MineBomberEngine _game;
        private Thread _renderThread;

        public Form1()
        {
            InitializeComponent();

            Init();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void StartRender()
        {
            while (true)
            {
                _game.Render();
                Application.DoEvents();
            }
        }

        private void Init()
        {
            Width = MineBomberEngine.SCREEN_WIDTH;
            Height = MineBomberEngine.SCREEN_HEIGHT;

            _game = new MineBomberEngine(this);
            
            _game.InitDraw();

            _game.CreateSurfaceFromBitmapN("mb_mans2.png", 0);//mb_mans.png
            _game.CreateSurfaceFromBitmapN("mb_elements.png", 1);
            _game.CreateSurfaceFromBitmapN("mb_fire.png", 2);
            _game.CreateSurfaceFromBitmapN("mb_stat.png", 3);
//
//              runscript('player 1 create 1 3 0 0');
//  runscript('player 2 create 1 5 1 0');
//  runscript('player 3 create 1 7 2 0');
            _game.MySprites[0] = new MySprite(
                _game, 
                new Point(1, 3), 
                MineBomberEngine.CELL_SIZE,
                MineBomberEngine.CELL_SIZE,
                0,
                0);

            _game.MySprites[1] = new MySprite(
                _game,
                new Point(1, 5),
                MineBomberEngine.CELL_SIZE,
                MineBomberEngine.CELL_SIZE,
                1,
                0);
            
            _game.GameMap.Load(null);

            _renderThread = new Thread(StartRender);
            _renderThread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _renderThread.Abort();
            _game.Exit();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'a':
                case 'A':
                    _game.MySprites[1].MoveDirection = Sprite.Direction.Left;
                    break;
                case 's':
                case 'S':
                    _game.MySprites[1].MoveDirection = Sprite.Direction.Bottom;
                    break;
                case 'd':
                case 'D':
                    _game.MySprites[1].MoveDirection = Sprite.Direction.Right;
                    break;
                case 'w':
                case 'W':
                    _game.MySprites[1].MoveDirection = Sprite.Direction.Top;
                    break;
                default:
                    _game.MySprites[1].MoveDirection = Sprite.Direction.None;
                    break;
            }

            _game.MySprites[1].Go();
            Text = e.KeyChar.ToString();
        }
    }
}
