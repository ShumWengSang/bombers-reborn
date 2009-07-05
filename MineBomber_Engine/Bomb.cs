using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX.DirectDraw;

namespace MineBomber_Engine
{
    public class Bomb : Sprite
    {
        public int Power { get; set; }

        public void Damage(int num)
        {
            int xn, yn;

            for(int d = 0; d <= Power; d++)
            {
                for(int i=0; i<= 360; i++)
                {
                    xn = Location.X + d * (int)Math.Cos(i / Math.PI / 180);
                    yn = Location.Y + d * (int)Math.Sin(i / Math.PI / 180);

                    if((xn > 0) && (yn > 0))
                    {
                        (FEngine.GameMap.DamMap[xn][yn])++;
                    }
                }
            }

            for(yn = 0; yn < MineBomberEngine.MAP_HEIGHT_CELLS; yn++)
            {
                for(xn = 0; xn < MineBomberEngine.MAP_WIDTH_CELLS; xn++)
                {
                    if(FEngine.GameMap.MapChars[xn][yn] >= '7' &&
                       FEngine.GameMap.MapChars[xn][yn] <= 'F' )
                    {
                        FEngine.GameMap.ModMap[xn][yn] += FEngine.GameMap.DamMap[xn][yn] / 10;
                    }

                    if (FEngine.GameMap.MapChars[xn][yn] >= '2' &&
                       FEngine.GameMap.MapChars[xn][yn] <= '6')
                    {
                        FEngine.GameMap.ModMap[xn][yn] += FEngine.GameMap.DamMap[xn][yn] / 5;
                    }

                    if(FEngine.GameMap.ModMap[xn][yn] > 2)
                    {
                        FEngine.GameMap.MapChars[xn][yn] = '0';
                        FEngine.GameMap.ModMap[xn][yn] = 0;
                    }
                }
            }

            for(int mySpriteNumber = 0; mySpriteNumber < FEngine.MySprites.Length; mySpriteNumber++)
            {
                Point mySpritePoint = FEngine.MySprites[mySpriteNumber].MapPoint;
                FEngine.MySprites[mySpriteNumber].Live -= FEngine.GameMap.DamMap[mySpritePoint.X][mySpritePoint.Y];
            }
            
            FEngine.Bombs.RemoveAt(num);
            FEngine.GameMap.Draw();
            /*   
   zeromemory(@fengine.map.dammap,sizeof(fengine.map.dammap));
     
      
   {$ifdef sound}
   BASS_SamplePlayEx(fengine.SHELLEXPLODE, 0, -1, 50, (x*200 div 65)*(-1)+100, False);
   {$endif}
*/
        }

        public bool Draw(int num)
        {
            bool result = true;

            switch (Faze)
            {
                case 0:
                case 1:
                case 2:
                    _aRect = new Rectangle(
                        (Faze + 5) * MineBomberEngine.CELL_SIZE,
                        (_stype + 2) * MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE);

                    _dRect = new Rectangle(
                        Location.X * MineBomberEngine.CELL_SIZE,
                        Location.Y * MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE,
                        MineBomberEngine.CELL_SIZE);

                    base.Draw();
                    break;
                case 3:
                case 4:
                case 5:
                    Speed -= 70;

                    _aRect = new Rectangle(
                        (Faze - 3) * 30,
                        (Faze - 3) * 30 + 30,
                        0,
                        30);

                    _dRect = new Rectangle(
                        Location.X * MineBomberEngine.CELL_SIZE - 15*Power + 6,
                        Location.Y * MineBomberEngine.CELL_SIZE - 15*Power + 6 + 50,
                        Location.X * MineBomberEngine.CELL_SIZE + 15 * Power + 6,
                        Location.Y * MineBomberEngine.CELL_SIZE + 15 * Power + 6 + 50);

                    FEngine.BackSurface.DrawFast(0,0, FEngine.SpriteSurface[2], _aRect, DrawFastFlags.SourceColorKey);
                    break;
                case 6:
                    result = false;
                    Damage(num);
                    break;
            }

            long lastTick = FEngine.Tick;
            if(Tick + Speed < lastTick)
            {
                --Faze;
                Tick = lastTick;
            }

            return result;
        }
    }
}
