using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LedMatrixCSharp.Utils;

namespace LedMatrixCSharp.View
{
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
        private int _Width = 0;
        private int _Height = 0;
        private int _X = 0;
        private int _Y = 0;


        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
                pixelMap = new CanvasColor[Width, Height];
            }
        }
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
                pixelMap = new CanvasColor[Width, Height];
            }
        }

        public int X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }
        public int Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }

        // private protected is not available in core 2.2 :( this is sad
        protected internal CanvasColor[,] pixelMap = new CanvasColor[0,0];

        public Canvas() {}

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

        public void SetPixel(int x, int y, CanvasColor color)
        {
            // Return if outside of Matrix
            if (OutsideCanvas(x, y)) return;
            this.pixelMap[x, y] = color;
        }

        private bool OutsideCanvas(int x, int y)
        {
            return x > Width - 1 || y > Height - 1 ||
                   x < 0 || y < 0;
        }

        public CanvasColor GetPixel(int x, int y)
        {
            return this.pixelMap[x, y];
        }

        public virtual void Clear()
        {
            this.pixelMap = new CanvasColor[Width, Height];
        }
    }
}