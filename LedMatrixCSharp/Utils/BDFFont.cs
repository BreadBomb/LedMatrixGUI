using LedMatrixCSharp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LedMatrixCSharp.Utils
{
    public class FontCache
    {
        private static FontCache _Instance = null;

        public static FontCache Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new FontCache();
                }

                return _Instance;
            }
        }

        public Dictionary<string, BDFFont> Fonts = new Dictionary<string, BDFFont>();
    }

    public class BDFFont
    {
        public static BDFFont LoadFont4x6()
        {
            BDFFont font = new BDFFont();

            if (!FontCache.Instance.Fonts.ContainsKey(Path.Combine(Environment.CurrentDirectory, "Fonts", "4x6.bdf")))
            {
                font.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "4x6.bdf"));
                FontCache.Instance.Fonts.Add(Path.Combine(Environment.CurrentDirectory, "Fonts", "4x6.bdf"), font);
                Console.WriteLine(font.glyphs.Count);
                return font;
            }

            return FontCache.Instance.Fonts[Path.Combine(Environment.CurrentDirectory, "Fonts", "4x6.bdf")];
        }

        public static BDFFont LoadFont5x7()
        {
            BDFFont font = new BDFFont();

            if (!FontCache.Instance.Fonts.ContainsKey(Path.Combine(Environment.CurrentDirectory, "Fonts", "5x7.bdf")))
            {
                var result = font.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "5x7.bdf"));
                FontCache.Instance.Fonts.Add(Path.Combine(Environment.CurrentDirectory, "Fonts", "5x7.bdf"), font);
                return font;
            }

            return FontCache.Instance.Fonts[Path.Combine(Environment.CurrentDirectory, "Fonts", "5x7.bdf")];
        }
        
        public static BDFFont LoadFont8x13()
        {
            BDFFont font = new BDFFont();

            if (!FontCache.Instance.Fonts.ContainsKey(Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13.bdf")))
            {
                var result = font.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13.bdf"));
                FontCache.Instance.Fonts.Add(Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13.bdf"), font);
                return font;
            }

            return FontCache.Instance.Fonts[Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13.bdf")];
        }

        public static BDFFont LoadFont8x13B()
        {
            BDFFont font = new BDFFont();

            if (!FontCache.Instance.Fonts.ContainsKey(Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13B.bdf")))
            {
                var result = font.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13B.bdf"));
                FontCache.Instance.Fonts.Add(Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13B.bdf"), font);
                return font;
            }

            return FontCache.Instance.Fonts[Path.Combine(Environment.CurrentDirectory, "Fonts", "8x13B.bdf")];
        }

        public static BDFFont LoadFont9x15()
        {
            BDFFont font = new BDFFont();

            if (!FontCache.Instance.Fonts.ContainsKey(Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15.bdf")))
            {
                var result = font.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15.bdf"));
                FontCache.Instance.Fonts.Add(Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15.bdf"), font);
                return font;
            }

            return FontCache.Instance.Fonts[Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15.bdf")];
        }

        public static BDFFont LoadFont9x15B()
        {
            BDFFont font = new BDFFont();

            if (!FontCache.Instance.Fonts.ContainsKey(Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15B.bdf")))
            {
                var result = font.LoadFont(Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15B.bdf"));
                FontCache.Instance.Fonts.Add(Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15B.bdf"), font);
                return font;
            }

            return FontCache.Instance.Fonts[Path.Combine(Environment.CurrentDirectory, "Fonts", "9x15B.bdf")];
        }

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

        public BDFFont()
        {
        }

        public int Height { get; set; }

        public int BaseLine { get; set; }

        public int getCharacterWidth(int unicode)
        {
            var glyph = FindGlyph(unicode);
            return glyph?.DeviceWidth ?? -1;
        }

        public bool LoadFont(Stream stream)
        {
            if (stream == default(Stream)) return false;
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

        public bool DrawGlyph(Canvas canvas, int x, int y, CanvasColor color, int unicode)
        {
            return DrawGlyph(canvas, x, y, color, null, unicode);
        }

        public bool DrawGlyph(Canvas canvas, int x, int y, CanvasColor color, CanvasColor background, int unicode)
        {
            var glyph = FindGlyph(unicode);
            if (glyph == null) glyph = FindGlyph(UnicodeReplacementCodepoint);
            if (glyph == null) return false;
            for (int _y = 0; _y < glyph.Height; _y++)
            {
                var bitmap = glyph.Bitmap[_y];
                var bitString = Convert.ToString(bitmap, 2);
                int[] bits = bitString.PadLeft(8, '0') // Add 0's from left
                    .Select(c => int.Parse(c.ToString())) // convert each char to int
                    .ToArray();
                for (int _x = 0; _x < glyph.DeviceWidth; _x++)
                {
                    if (bits[_x] == 1)
                    {
                        canvas.SetPixel(x + _x, y + _y, color);
                    }

                    if (bits[_x] == 0 && background != null)
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