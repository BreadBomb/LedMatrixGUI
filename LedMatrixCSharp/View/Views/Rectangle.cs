using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View.Views
{
    class Rectangle : View
    {
        private CanvasColor borderColor;
        private CanvasColor fillColor;

        public CanvasColor BorderColor { get; set; }
        public CanvasColor FillColor { get; set; }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor): base()
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Dimensions.Width = width;
            this.Dimensions.Height = height;
            this.BorderColor = borderColor;
        }

        public Rectangle(int x, int y, int width, int height, CanvasColor borderColor, CanvasColor fillColor): base()
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Dimensions.Width = width;
            this.Dimensions.Height = height;
            this.BorderColor = borderColor;
            this.FillColor = fillColor;
        }

        public override void Draw()
        {
            for (int x = Position.X; x < Position.X + Dimensions.Width; x++)
            {
                for (int y = Position.Y; y < Position.Y + Dimensions.Height; y++)
                {
                    CanvasColor drawColor = FillColor;
                    if (x == 0 || y == 0 || x == Position.X + Dimensions.Width - 1 || y == Position.Y + Dimensions.Height - 1) drawColor = BorderColor;
                    if (drawColor != null)
                        Canvas.SetPixel(new CanvasPosition { X = x, Y = y }, drawColor);
                }
            }
            base.Draw();
        }
    }
}
