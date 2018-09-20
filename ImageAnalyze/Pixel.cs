using System.Drawing;

namespace ImageAnalyze
{
    class Pixel
    {
        public int X { get; }
        public int Y { get; }
        public Color Color { get; }

        internal Pixel(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}