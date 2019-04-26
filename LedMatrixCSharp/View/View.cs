using LedMatrixCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View
{
    public class View
    {
        public CanvasPosition Position { get; set; } = new CanvasPosition();
        public Dimensions Dimensions { get; set; } = new Dimensions(32, 32);
        public Canvas Canvas;

        public View()
        {
            this.Canvas = new Canvas(Dimensions.Width, Dimensions.Height);
            this.Canvas.NeedRedraw += new Canvas.OnNeedRedraw(NeedRedraw);
        }

        private void NeedRedraw()
        {
            Canvas.Clear();
        }

        public virtual void Draw()
        {
            Renderer.Renderer.RenderCache.Add(Canvas);
        }
    }
}
