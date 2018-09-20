using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageAnalyze
{
    public class Main
    {
        public void Read(Bitmap image)
        {
            var width = image.Width;
            var height = image.Height;
            var pixels = new List<Pixel>();
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var color = image.GetPixel(w, h);
                    pixels.Add(new Pixel(w, h, color));
                }
            }

            var groups = pixels.GroupBy(x => x.Color, x => x,null);
        }
    }
}
