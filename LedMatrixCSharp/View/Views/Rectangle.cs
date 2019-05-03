using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;
using LedMatrixCSharp.Utils;
using System.Threading;

namespace LedMatrixCSharp.View.Views
{
    public class Rectangle : View
    {
        public CanvasColor BorderColor { get; set; }
        public CanvasColor FillColor { get; set; }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor): this(x, y, width, height, borderColor, null) { }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor, CanvasColor fillColor): base()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            this.BorderColor = borderColor;
            this.FillColor = fillColor;
        }

        public override void Update()
        {
            Clear();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CanvasColor drawColor = FillColor;
                    if (x == 0 || y == 0 || x == 0 + Width - 1 || y == 0 + Height - 1) drawColor = BorderColor;
                    if (drawColor != null)
                        SetPixel(x, y, drawColor);
                }
            }
        }
    }
}
