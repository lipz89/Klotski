using System.Collections.Generic;
using System.Drawing;

namespace ImageAnalyze
{
    class ColorComparer : IEqualityComparer<Color>
    {
        public bool Equals(Color x, Color y)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(Color obj)
        {
            throw new System.NotImplementedException(); 
        }
    }
}