using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LedMatrixCSharp.View.Views
{
    public class Image : View
    {
        private bool clearRequired = false;

        public Image(string path) : base()
        {
            this.readImage(path);
        }

        private void readImage(string path)
        {
            using (SixLabors.ImageSharp.Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(path)) //open the file and detect the file type and decode it
            {
                for (var x = 0; x < image.Width; x++)
                {
                    for (var y = 0; y < image.Height; y++)
                    {
                        var rawColor = image[x, y];
                        var color = new CanvasColor((int)rawColor.R, (int)rawColor.G, (int)rawColor.B);
                        Canvas.SetPixel(x, y, color);   
                    }
                }
            }
        }

        public override void OnBeforeDraw()
        {
            if (clearRequired)
            {
                Canvas.Clear();
            }
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}