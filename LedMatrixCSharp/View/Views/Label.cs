using LedMatrixCSharp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LedMatrixCSharp.View.Views
{
    class Label : View
    {
        BDFFont font;
        public Label(): base()
        {
            font = new BDFFont();
            Console.WriteLine(Path.Combine(Environment.CurrentDirectory, "Fonts", "4x6.bdf"));
            font.LoadFont4x6();
        }

        public override void Draw()
        {
            font.DrawGlyph(ref Canvas, new CanvasPosition(0, 0), CanvasColor.RED, 34);
            base.Draw();
        }
    }
}
