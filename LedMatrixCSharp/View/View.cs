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

        public View()
        {

        }

        public virtual void Update()
        {
        }
    }
}
