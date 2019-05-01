using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp
{
    public class MatrixApplication
    {
        public View.View Child;
        public bool NoRender = false;
        
        public MatrixApplication(bool noRender = false)
        {
            this.NoRender = noRender;
            var renderer = new Renderer.Renderer(NoRender);
            renderer.draw += new Renderer.Renderer.OnDraw(OnDraw);
            
        }

        private void OnDraw()
        {
            Child?.Draw();
        }
    }
}
