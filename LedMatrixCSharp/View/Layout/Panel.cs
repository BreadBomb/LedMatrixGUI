using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LedMatrixCSharp.View.Layout
{
    public class Panel : View
    {        
        protected void Concat(View view)
        {
            for ( int x = 0; x < view.Width; x++)
            {
                for (int y = 0; y < view.Height; y++)
                {
                    var pixel = view.GetPixel(x, y);
                    if (pixel != null && x + view.OffsetX <= Width && y + view.OffsetY <= Height)
                    {
                        SetPixel(x + view.OffsetX, y + view.OffsetY, pixel);
                    }
                }
            }
        }
    }
}
