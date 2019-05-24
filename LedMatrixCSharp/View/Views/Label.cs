using LedMatrixCSharp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Xml;

namespace LedMatrixCSharp.View.Views
{
    public class Label : View
    {
        private string _text = "";
        private BDFFont _font = new BDFFont();
        public CanvasColor Color { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                if (Font != null && value != _text)
                {
                        Width = _font.getCharacterWidth(value[0]) * value.Length;                        
                    Height = _font.Height;
                }
                this._text = value;
            }
        }

        public BDFFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                if (!string.IsNullOrEmpty(Text))
                {
                    foreach (char c in Text)
                    {
                        Width += _font.getCharacterWidth(c);                        
                    }
                }
                Height = _font.Height;
            }
        }
        
        public Label(string text, BDFFont font, CanvasColor color): base()
        {
            if (font == null) throw new ArgumentException("Font is not providet");
            Font = font;
            Text = text;
            Color = color;
        }

        public override void Update()
        {
            Clear();
            for (var i = 0; i < Text.Length; i++)
            {
                Font.DrawGlyph(this, Font.getCharacterWidth(0) * i, 0, Color, Text[i]);
            }
            base.Update();
        }
    }
}
