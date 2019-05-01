using LedMatrixCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View
{
    public class View
    {
        public CanvasPosition Position
        {
            get => Canvas.Offset;
            set => Canvas.Offset = value;
        }

        public Dimensions Dimensions
        {
            get => Canvas.Dimensions;
            set => Canvas.Dimensions = value;
        }
        public Canvas Canvas;
        private bool noRenderCache = false;

        public View()
        {
            this.Canvas = new Canvas();
        }

        public virtual void Draw()
        {
            if (!noRenderCache)
                Renderer.Renderer.RenderCache.Add(Canvas);
        }

        public void DrawInsideCanvas(ref Canvas canvas)
        {
            this.Canvas = canvas;
            noRenderCache = true;
        }
    }
}
