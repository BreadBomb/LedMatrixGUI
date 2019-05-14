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
        private Renderer.Renderer renderer;
        
        public MatrixApplication(bool noRender = false)
        {
            this.NoRender = noRender;
            renderer = new Renderer.Renderer(NoRender);
            renderer.update += new Renderer.Renderer.OnUpdate(OnUpdate);
            renderer.Start();
        }

        public virtual void OnUpdate()
        {
            Child?.Update();
            renderer.UpdateCanvas(Child);
        }
    }
}
