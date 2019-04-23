using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View
{
    public interface IView
    {
        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        void Draw();
        void OnBeforeDraw();
    }
}
