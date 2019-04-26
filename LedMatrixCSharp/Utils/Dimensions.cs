using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.Utils
{
    public class Dimensions
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;

        public Dimensions() { }

        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
