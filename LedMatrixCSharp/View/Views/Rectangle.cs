using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View.Views
{
    class Rectangle : View
    {
        public CanvasColor BorderColor { get; set; }
        public CanvasColor FillColor { get; set; } = null;

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor): base()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.BorderColor = borderColor;
        }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor, CanvasColor fillColor): base()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.BorderColor = borderColor;
            this.FillColor = fillColor;
        }

        public override void Draw()
        {
            for (int x = X; x < X + Width; x++)
            {
                for (int y = Y; y < Y + Height; y++)
                {
                    CanvasColor drawColor = FillColor;
                    if (x == 0 || y == 0 || x == X + Width - 1 || y == Y + Height - 1) drawColor = BorderColor;
                    if (drawColor != null)
                        Canvas.SetPixel(new CanvasPosition { X = x, Y = y }, drawColor);
                }
            }
            base.Draw();
        }
    }
}
