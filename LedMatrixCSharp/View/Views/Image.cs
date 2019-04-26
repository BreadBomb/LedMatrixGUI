using LedMatrixCSharp.Utils;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace LedMatrixCSharp.View.Views
{
    public class Image : View
    {
        private SixLabors.ImageSharp.Image<Rgba32> image;
        private string path;

        public new Dimensions Dimensions
        {
            get
            {
                return base.Dimensions;
            }
            set
            {
                base.Dimensions = value;
                this.readImage(Path);
            }
        }

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

        public string Path {
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

        private void readImage(string path)
        {
            image = SixLabors.ImageSharp.Image.Load(path);
            image.Mutate(ctx => {
                if (Dimensions.Width != image.Width || Dimensions.Height != image.Height)
                    ctx.Resize(Dimensions.Width, Dimensions.Height, KnownResamplers.Box);
            });
            base.Dimensions = new Dimensions(image.Width, image.Height);
        }

        public override void Draw()
        {
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var rawColor = image[x, y];
                    var color = new CanvasColor((int)rawColor.R, (int)rawColor.G, (int)rawColor.B);
                    Canvas.SetPixel(Position.X + x, Position.Y + y, color);
                }
            }
            base.Draw();
        }
    }
}