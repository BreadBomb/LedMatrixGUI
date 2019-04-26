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

        public new CanvasPosition Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                this._text = value;
                if (Font != null)
                {
                    this.Dimensions.Width = Text.Length * Font.getCharacterWidth(0);
                    this.Dimensions.Height = _font.Height;
                }
            }
        }

        public BDFFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                this.Dimensions.Width = Text.Length * Font.getCharacterWidth(0);
                this.Dimensions.Height = _font.Height;
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
                Font.DrawGlyph(ref Canvas, new CanvasPosition(Position.X + Font.getCharacterWidth(0) * i, Position.Y + Font.Height), CanvasColor.RED, Text[i]);
            }
            base.Draw();
        }
    }
}
