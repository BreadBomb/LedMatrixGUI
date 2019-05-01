using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LedMatrixCSharp.View.Views
{
    public class Ellipse: View
    {
        public CanvasColor BorderColor { get; set; }

        private double rx;
        private double ry;

        public Ellipse(int x, int y, double width, double height, CanvasColor borderColor) : base()
        {
            Position.X = x;
            Position.Y = y;
            this.rx = width / 2;
            this.ry = height / 2;
            Dimensions.Width = (int)width;
            Dimensions.Height = (int)height;
            this.BorderColor = borderColor;
        }
        
        public override void Draw()
        {
            double dx, dy, d1, d2, x, y;
            x = 0;
            y = ry;

            // Initial decision parameter of region 1 
            d1 = (ry * ry) - (rx * rx * ry) +
                 (0.25f * rx * rx);
            dx = 2 * ry * ry * x;
            dy = 2 * rx * rx * y;

            // For region 1 
            while (dx < dy)
            {
                Canvas.SetPixel((int)(x +Position.X), (int)(y + Position.Y), BorderColor);       
                Canvas.SetPixel((int)(-x +Position.X), (int)(y + Position.Y), BorderColor);       
                Canvas.SetPixel((int)(x +Position.X), (int)(-y + Position.Y), BorderColor);       
                Canvas.SetPixel((int)(-x +Position.X), (int)(-y + Position.Y), BorderColor);       

                // Checking and updating value of 
                // decision parameter based on algorithm 
                if (d1 < 0)
                {
                    x++;
                    dx = dx + (2 * ry * ry);
                    d1 = d1 + dx + (ry * ry);
                }
                else
                {
                    x++;
                    y--;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d1 = d1 + dx - dy + (ry * ry);
                }
            }

            // Decision parameter of region 2 
            d2 = ((ry * ry) * ((x + 0.5f) * (x + 0.5f)))
                 + ((rx * rx) * ((y - 1) * (y - 1)))
                 - (rx * rx * ry * ry);

            // Plotting points of region 2 
            while (y >= 0)
            {
                // printing points based on 4-way symmetry 
                Canvas.SetPixel((int)(x +Position.X), (int)(y + Position.Y), BorderColor);       
                Canvas.SetPixel((int)(-x +Position.X), (int)(y + Position.Y), BorderColor);       
                Canvas.SetPixel((int)(x +Position.X), (int)(-y + Position.Y), BorderColor);       
                Canvas.SetPixel((int)(-x +Position.X), (int)(-y + Position.Y), BorderColor); 

                // Checking and updating parameter 
                // value based on algorithm 
                if (d2 > 0)
                {
                    y--;
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + (rx * rx) - dy;
                }
                else
                {
                    y--;
                    x++;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + dx - dy + (rx * rx);
                }
            }

            base.Draw();
        }
    }
}