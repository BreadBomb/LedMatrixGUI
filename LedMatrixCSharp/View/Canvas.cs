using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LedMatrixCSharp.Utils;

namespace LedMatrixCSharp.View
{
    public class CanvasPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CanvasPosition()
        {
        }

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
        public static CanvasColor YELLOW => new CanvasColor(249, 237, 105);
        public static CanvasColor GREEN => new CanvasColor(23, 185, 120);
        public static CanvasColor ORANGE => new CanvasColor(250, 181, 122);
        public static CanvasColor PINK => new CanvasColor(255, 46, 99);
        public static CanvasColor BLUE => new CanvasColor(37, 59, 110);

        public CanvasColor(int R, int G, int B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
    }

    public class Canvas
    {
        private CanvasPosition _offset = new CanvasPosition() {X = 0, Y = 0};
        private Dimensions _dimensions = new Dimensions();

        public Dimensions Dimensions
        {
            get { return this._dimensions; }
            set
            {
                this._dimensions = value;
                this.pixelMap = new CanvasColor[this._dimensions.Width, this._dimensions.Height];
            }
        }

        public CanvasPosition Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        private CanvasColor[,] pixelMap = new CanvasColor[0,0];

        public Canvas() {}

        public void Fill(CanvasColor color)
        {
            for (var x = 0; x < Dimensions.Width; x++)
            {
                for (var y = 0; y < Dimensions.Height; y++)
                {
                    SetPixel(x, y, color);
                }
            }
        }

        public void SetPixel(CanvasPosition pos, CanvasColor color)
        {
            SetPixel(pos.X, pos.Y, color);
        }

        public void SetPixel(int x, int y, CanvasColor color)
        {
            // Return if outside of Matrix
            if (OutsideCanvas(x, y)) return;
            this.pixelMap[x, y] = color;
        }

        private bool OutsideCanvas(int x, int y)
        {
            return x > Dimensions.Width - 1 || y > Dimensions.Height - 1 ||
                   x < 0 || y < 0;
        }

        public CanvasColor GetPixel(CanvasPosition pos)
        {
            return this.pixelMap[pos.X, pos.Y];
        }

        public void Clear()
        {
            this.pixelMap = new CanvasColor[this.Dimensions.Width, this.Dimensions.Height];
        }
    }
}