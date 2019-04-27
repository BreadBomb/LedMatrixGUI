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
        private CanvasPosition offset = new CanvasPosition() { X = 0, Y = 0 };
        private Dimensions dimensions = new Dimensions();

        public Dimensions Dimensions {
            get
            {
                return this.dimensions;
            }
            private set
            {
                this.dimensions = value;
                this.NeedRedraw.Invoke();
            }
        }
        public CanvasPosition Offset {
            get
            {
                return this.offset;
            }
            set
            {
                this.offset = value;
                this.NeedRedraw.Invoke();
            }
        } 
        private CanvasColor[,] pixelMap;
        public delegate void OnNeedRedraw();
        public event OnNeedRedraw NeedRedraw;

        public Canvas(int width, int height)
        {
            this.dimensions = new Dimensions(width, height);
            this.pixelMap = new CanvasColor[this.Dimensions.Width, this.Dimensions.Height];
        }

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
            if (pos.X + this.Offset.X > Dimensions.Width - 1 || pos.Y + this.Offset.Y > Dimensions.Height - 1 || pos.X + this.Offset.X < 0 || pos.Y + this.Offset.Y < 0) return;
            this.pixelMap[pos.X + this.Offset.X, pos.Y + this.Offset.Y] = color;
        }

        public void SetPixel(int x, int y, CanvasColor color)
        {
            if (x + this.Offset.X > Dimensions.Width - 1 || y + this.Offset.Y > Dimensions.Height - 1 || x + this.Offset.X < 0 || y + this.Offset.Y < 0) return;
            this.pixelMap[x + this.Offset.X, y + this.Offset.Y] = color;
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
