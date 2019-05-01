using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;
using LedMatrixCSharp.Utils;

namespace LedMatrixCSharp.View.Views
{
    public class Rectangle : View
    {
        public CanvasColor BorderColor { get; set; }
        public CanvasColor FillColor { get; set; }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor): base()
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Dimensions = new Dimensions(width, height);
            this.BorderColor = borderColor;
        }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor, CanvasColor fillColor): base()
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Dimensions = new Dimensions(width, height);
            this.BorderColor = borderColor;
            this.FillColor = fillColor;
        }

        public override void Draw()
        {
            for (int x = 0; x < Dimensions.Width; x++)
            {
                for (int y = 0; y < Dimensions.Height; y++)
                {
                    CanvasColor drawColor = FillColor;
                    if (x == 0 || y == 0 || x == 0 + Dimensions.Width - 1 || y == 0 + Dimensions.Height - 1) drawColor = BorderColor;
                    if (drawColor != null)
                        Canvas.SetPixel(new CanvasPosition { X = x, Y = y }, drawColor);
                }
            }
            base.Draw();
        }
    }
}
