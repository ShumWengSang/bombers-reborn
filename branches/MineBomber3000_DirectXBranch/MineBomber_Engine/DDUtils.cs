using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX.DirectDraw;

namespace MineBomber_Engine
{
    internal class DDUtils
    {
        /// <summary>
        /// Gets Surface from bitmap file.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="pathToBitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Surface LoadBitmap(
            Device device,
            string pathToBitmap,
            int width,
            int height)
        {
            Surface result = null;
            var image = new Bitmap(pathToBitmap);
            if(File.Exists(pathToBitmap))
            {
                var surfaceDescription = new SurfaceDescription
                                             {
                                                 Width = image.Width,
                                                 Height = image.Height,
                                                 SurfaceCaps = {OffScreenPlain = true}
                                             };
                
                result = new Surface(pathToBitmap, surfaceDescription, device);
            }

            return result;
        }
    }
}
