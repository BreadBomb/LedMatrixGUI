using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LedMatrixCSharp.View
{
    public class CanvasPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CanvasPosition() { }

        public CanvasPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public class CanvasColor
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public static CanvasColor WHITE => new CanvasColor(255, 255, 255);
        public static CanvasColor GRAY => new CanvasColor(57, 62, 70);
        public static CanvasColor RED => new CanvasColor(232, 69, 69);

        public CanvasColor(int R, int G, int B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
    }

    public class Canvas
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public CanvasPosition Offset { get; set; } = new CanvasPosition() { X = 0, Y = 0 };
        private CanvasColor[,] pixelMap;

        public Canvas(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.pixelMap = new CanvasColor[this.Width, this.Height];
        }

        public void Fill(CanvasColor color)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    SetPixel(x, y, color);
                }
            }
        }

        public void SetPixel(CanvasPosition pos, CanvasColor color)
        {
            if (pos.X + this.Offset.X > Width - 1 || pos.Y + this.Offset.Y > Height - 1) return;
            this.pixelMap[pos.X + this.Offset.X, pos.Y + this.Offset.Y] = color;
        }

        public void SetPixel(int x, int y, CanvasColor color)
        {
            if (x + this.Offset.X > Width - 1 || y + this.Offset.Y > Height - 1) return;
            this.pixelMap[x + this.Offset.X, y + this.Offset.Y] = color;
        }

        public CanvasColor GetPixel(CanvasPosition pos)
        {
            return this.pixelMap[pos.X, pos.Y];
        }

        public void Clear()
        {
            this.pixelMap = new CanvasColor[this.Width, this.Height];
        }
    }
}
