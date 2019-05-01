using LedMatrixCSharp.View;
using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LedMatrixCSharp.Renderer
{
    public class Renderer
    {
        private RGBLedMatrix matrix;
        private RGBLedCanvas canvas;
        private Thread _thread;
        private bool NoRender = false;

        public delegate void OnDraw();
        public event OnDraw draw;

        public static List<Canvas> RenderCache = new List<Canvas>();

        public Renderer(bool noRender = false)
        {
            this.NoRender = noRender;
            RGBLedMatrixOptions ledMatrixOptions = new RGBLedMatrixOptions();
            ledMatrixOptions.Cols = 32;
            ledMatrixOptions.Rows = 32;
            ledMatrixOptions.PwmLsbNanoseconds = 100; 
                
            if (!NoRender)
            {
                matrix = new RGBLedMatrix(ledMatrixOptions);
                canvas = matrix.CreateOffscreenCanvas();
                Console.WriteLine("Matrix and Canvas initalized");
            }

            _thread = new Thread(new ThreadStart(Render));
            _thread.Start();
        }

        private void Render()
        {
            while(true)
            {
                MergeCanvasCache();
                if (!NoRender)
                {
                    matrix.SwapOnVsync(canvas);
                    canvas.Clear();
                }
                RenderCache.Clear();
                draw?.Invoke();
                Thread.Sleep(16);
            }
        }
        private void MergeCanvasCache()
        {
            RenderCache.Reverse();
            foreach (var c in RenderCache)
            {
                for (var x = 0; x < c.Dimensions.Width; x++)
                {
                    for (var y = 0; y < c.Dimensions.Height; y++)
                    {
                        var color = c.GetPixel(new CanvasPosition { X = x, Y = y });
                        if (color != null)
                        {
                            if (!NoRender)
                            {
                                canvas.SetPixel(x + c.Offset.X, y + c.Offset.Y, new Color(color.R, color.G, color.B));
                            }
                        }
                    }
                }
            }
        }
    }
}
