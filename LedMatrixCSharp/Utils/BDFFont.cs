using LedMatrixCSharp.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LedMatrixCSharp.Utils
{

    public class BDFFont
    {
        public class Glyph
        {
            public int DeviceWidth { get; set; }
            public int DeviceHeight { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
            public int[] Bitmap { get; set; }
        }

        private Dictionary<int, Glyph> glyphs = new Dictionary<int, Glyph>();

        public bool LoadFont4x6() => this.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "4x6.bdf"));
        public bool LoadFont5x7() => this.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "5x7.bdf"));

        public BDFFont() { }

        public int Height { get; set; }

        public int BaseLine { get; set; }

        public int getCharacterWidth(int unicode)
        {
            var glyph = FindGlyph(unicode);
            return glyph != null ? glyph.DeviceWidth : -1;
        }

        public bool LoadFont(Stream stream)
        {
            if (stream == default(Stream)) return false;
            if (stream == null) return false;
            var streamReader = new StreamReader(stream);
            int codepoint = 0;
            int dummy;
            Glyph currentGlyph = null;
            int row = 0;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line.StartsWith("FONTBOUNDINGBOX"))
                {
                    var values = line.Split(' ');
                    dummy = int.Parse(values[1]);
                    Height = int.Parse(values[2]);
                    dummy = int.Parse(values[3]);
                    BaseLine = int.Parse(values[4]);
                }
                else if (line.StartsWith("ENCODING"))
                {
                    var values = line.Split(' ');
                    codepoint = int.Parse(values[1]);
                }
                else if (line.StartsWith("DWIDTH"))
                {
                    var values = line.Split(' ');
                    currentGlyph = new Glyph();
                    currentGlyph.DeviceWidth = int.Parse(values[1]);
                    currentGlyph.DeviceHeight = int.Parse(values[2]);
                }
                else if (line.StartsWith("BBX"))
                {
                    var values = line.Split(' ');
                    currentGlyph.Width = int.Parse(values[1]);
                    currentGlyph.Height = int.Parse(values[2]);
                    currentGlyph.OffsetX = int.Parse(values[3]);
                    currentGlyph.OffsetY = int.Parse(values[4]);
                    currentGlyph.Bitmap = new int[currentGlyph.Height];
                    row = -1;
                }
                else if (line.StartsWith("BITMAP"))
                {
                    row = 0;
                }
                else if (currentGlyph != null && row >= 0 && row < currentGlyph.Height)
                {
                    currentGlyph.Bitmap[row] = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    row++;
                }
                else if (line.StartsWith("ENDCHAR"))
                {
                    glyphs.Add(codepoint, currentGlyph);
                    currentGlyph = null;
                }
            }
            streamReader.Close();
            stream.Close();

            return true;
        }

        public bool LoadFont(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open);
            return LoadFont(fileStream);
        }

        public Glyph FindGlyph(int unicode)
        {
            return this.glyphs[unicode];
        }

        public bool DrawGlyph(ref Canvas canvas, int x, int y, CanvasColor color, int unicode)
        {
            return DrawGlyph(ref canvas, x, y, color, null, unicode);
        }

        public bool DrawGlyph(ref Canvas canvas, int x, int y, CanvasColor color, CanvasColor background, int unicode)
        {
            var glyph = FindGlyph(unicode);
            if (glyph == null) glyph = FindGlyph(UnicodeReplacementCodepoint);
            if (glyph == null) return false;
            for(int _y = 0; y < glyph.Height; y++)
            {
                var bitmap = glyph.Bitmap[y];
                var bitString = Convert.ToString(bitmap, 2);
                int[] bits = bitString.PadLeft(8, '0') // Add 0's from left
                    .Select(c => int.Parse(c.ToString())) // convert each char to int
                    .ToArray();
                for(int _x = 0; x < glyph.DeviceWidth; x++)
                {
                    if (bits[x] == 1)
                    {
                        canvas.SetPixel(x + _x, y + _y, color);
                    }
                    if (bits[x] == 0 && background != null)
                    {
                        canvas.SetPixel(x + _x, y + _y, background);
                    }
                }
            } 
            return true;
        }

        const int UnicodeReplacementCodepoint = 0xFFFD;
    }
}
