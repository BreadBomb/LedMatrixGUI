using LedMatrixCSharp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LedMatrixCSharp.View.Views
{
    public class Label : View
    {
        private string _text;
        private BDFFont _font = new BDFFont();

        public string Text
        {
            get { return _text; }
            set
            {
                this._text = value;
                if (Font != null)
                {
                    this.Width = Text.Length * Font.getCharacterWidth(0);
                    this.Height = _font.Height;
                }
            }
        }

        public BDFFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                this.Width = Text.Length * Font.getCharacterWidth(0);
                this.Height = _font.Height;
            }
        }
        

        public Label(string text): base()
        {
            Font.LoadFont4x6();
            this.Text = text;
        }

        public override void Draw()
        {
            for (var i = 0; i < Text.Length; i++)
            {
                Font.DrawGlyph(ref Canvas, new CanvasPosition(X + Font.getCharacterWidth(0)*i, Y + Font.Height), CanvasColor.RED, Text[i]);
            }
            base.Draw();
        }
    }
}
