using LedMatrixCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LedMatrixCSharp.View
{
    public class View : Canvas
    {
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public CanvasColor BackgroundColor = null;
        
        public View()
        {
        }

        public override void Clear()
        {
            base.Clear();
            if (BackgroundColor != null)
            {
                Fill(BackgroundColor);
            }
        }

        public virtual void Update()
        {
        }
    }
}
