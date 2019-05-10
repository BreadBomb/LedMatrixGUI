using LedMatrixCSharp.Utils;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System.IO;

namespace LedMatrixCSharp.View.Views
{
    public class Image : View
    {
        private SixLabors.ImageSharp.Image<Rgba32> image;
        private string path;

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
                this.readImage(this.path);
            }
        }

        public Image(string path) : base()
        {
            this.Path = path;
            this.readImage(path);
        }

        public Image(Stream stream) : base()
        {
            this.readImage(stream);
        }

        private void readImage(string path)
        {
            image = SixLabors.ImageSharp.Image.Load(path);
            if (Width == 0)
            {
                Width = image.Width + X;
            }
            if (Height == 0)
            {
                Height = image.Height + Y;
            }
            image.Mutate(ctx =>
            {
                if (Width != image.Width || Height != image.Height)
                    ctx.Resize(Width, Height, KnownResamplers.Box);
            });
        }

        private void readImage(Stream stream)
        {
            image = SixLabors.ImageSharp.Image.Load(stream);
            if (Width == 0)
            {
                Width = image.Width;
            }
            if (Height == 0)
            {
                Height = image.Height;
            }
            image.Mutate(ctx =>
            {
                if (Width != image.Width || Height != image.Height)
                    ctx.Resize(Width, Height, KnownResamplers.Box);
            });
        }

        public override void Update()
        {
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var rawColor = image[x, y];
                    var color = new CanvasColor((int)rawColor.R, (int)rawColor.G, (int)rawColor.B);
                    SetPixel(x, y, color);
                }
            }
            base.Update();
        }
    }
}