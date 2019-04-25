using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View
{
    public class View
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; } = 32;
        public int Height { get; set; } = 32;
        public Canvas Canvas;

        public View()
        {
            this.Canvas = new Canvas(Width, Height);
            Renderer.Renderer.beforeDraw += new Renderer.Renderer.OnBeforeDraw(OnBeforeDraw);
        }

        public virtual void Draw()
        {
            Renderer.Renderer.RenderCache.Add(Canvas);
        }

        public virtual void OnBeforeDraw()
        {
            Canvas.Clear();
        }
    }
}
