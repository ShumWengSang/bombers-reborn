using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace MineBomber_Engine
{
    public class Map
    {
        public int[][] ModMap { get; set; }
        public int[][] DamMap { get; set;}
        public char[][] MapChars { get; set;}
        public Point[][] MapElem { get; set;}
        public MineBomberEngine FEngine { get; set; }

        public Map(MineBomberEngine engine, int width, int height)
        {
            FEngine = engine;
            MapSize = new Size(width, height);

            InitMap(width, height);
        }

        private void InitMap(int width, int height)
        {
            MapChars = new char[width][];
            ModMap = new int[width][];
            DamMap = new int[width][];
            
            for(int i=0; i<width; i++)
            {
                ModMap[i] = new int[height];
                DamMap[i] = new int[height];
                MapChars[i] = new char[height];
                for (int k = 0; k < height; k++) 
                {
                    MapChars[i][k] = '0';
                }
            }
        
        }

        public Size MapSize { get; private set; }
        public List<Point> Teleports { get; set; }
        public List<Button> Buttons { get; set; }

        public void Draw()
        {
            for(int x = 0; x < MineBomberEngine.MAP_WIDTH_CELLS; x++)
            {
                for(int y = 0; y < MineBomberEngine.MAP_HEIGHT_CELLS; y++)
                {
                    int d = 0;
                    char c = MapChars[x][y];

                    if(((c > 'A') && (c < 'F')) ||
                        ((c > '7') && (c < '9')))
                    {
                        d = ModMap[x][y];
                    }
                    
                    var mapRect = new Rectangle(
                        MapElem[c - 48][d].X*MineBomberEngine.CELL_SIZE,
                        MapElem[c - 48][d].Y*MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE);

                    var dRect = new Rectangle(
                        x*MineBomberEngine.CELL_SIZE,
                        y*MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE);

                    FEngine.MapSurface.Draw(
                        dRect,
                        FEngine.SpriteSurface[1],
                        mapRect,
                        0);
                }
            }
        }

        public void Load(string pathToMap)
        {
            if(string.IsNullOrEmpty(pathToMap)) {
                pathToMap = "Maps/Crumble.mne";}

            var tempMapElem = new List<Point[]>();

            using (var reader = File.OpenText("map.bmd"))
            {
                string mapElemsDictionary;
                while((mapElemsDictionary = reader.ReadLine()) != null)
                {
                    string[] mapElems = mapElemsDictionary.Split(' ');
                    tempMapElem.Add(new Point[3]
                                        {
                                            new Point(int.Parse(mapElems[1]), int.Parse(mapElems[2])),
                                            new Point(int.Parse(mapElems[3]), int.Parse(mapElems[4])),
                                            new Point(int.Parse(mapElems[5]), int.Parse(mapElems[6]))
                                        });
                }
            }
            MapElem = tempMapElem.ToArray();

            using(var reader = File.OpenRead(pathToMap))
            {
                for(int y = 0; y < MineBomberEngine.MAP_HEIGHT_CELLS; y++)
                {
                    //TODO: Rework this indus logic
                    var line = new byte[MineBomberEngine.MAP_WIDTH_CELLS];
                    reader.Read(line, 0, line.Length);
                    var lineChars = Encoding.UTF7.GetChars(line);
                    for (int x = 0; x < MineBomberEngine.MAP_WIDTH_CELLS; x++)
                    {
                        MapChars[x][y] = lineChars[x];
                    }
                    reader.Position += 2; //Pass /r/n sequence
                }
            }

            Draw();
        }

        public void AddTeleport(){}

        public void AddButton(int mapX, int mapY)
        {
            var newButton = new Button()
                            {
                                Location = new Point(mapX, mapY),

                            };

            for(int dx = -1; dx <= 1; dx++)
            {
                for(int dy = -1; dy <= 1; dy++)
                {
                    if(dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    if(MapChars[mapX + dx][mapY + dy] == '1')
                    {
                        newButton.Doors.Add(new Point(mapX + dx, mapY + dy));
                    }
                }
            }
        }
    }
}
