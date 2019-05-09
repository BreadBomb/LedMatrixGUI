using LedMatrixCSharp.View;
using rpi_rgb_led_matrix_sharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using Unosquare.Swan;

namespace LedMatrixCSharp.Renderer
{
    public class Renderer
    {
        private RGBLedMatrix matrix;
        private RGBLedCanvas canvas;
        private Thread _thread;
        private bool NoRender = false;

        public delegate void OnUpdate();
        public event OnUpdate update;

        private readonly ConcurrentQueue<Canvas> _queue = new ConcurrentQueue<Canvas>();
        private readonly AutoResetEvent _signal = new AutoResetEvent(false);

        public System.Timers.Timer updateTimer = new System.Timers.Timer();

        public Renderer(bool noRender = false)
        {
            this.NoRender = noRender;
            RGBLedMatrixOptions ledMatrixOptions = new RGBLedMatrixOptions();
            ledMatrixOptions.Cols = 32;
            ledMatrixOptions.Rows = 32;
            ledMatrixOptions.PwmLsbNanoseconds = 50;
                
            if (!NoRender)
            {
                matrix = new RGBLedMatrix(ledMatrixOptions);                
                canvas = matrix.CreateOffscreenCanvas();
                Console.WriteLine("Matrix and Canvas initalized");
            }

            updateTimer.Elapsed += new ElapsedEventHandler(_OnUpdate);
            updateTimer.Interval = 1000/60;

            _thread = new Thread(new ThreadStart(Render));
            
        }

        public void Start()
        {
            updateTimer.Start();
            _thread.Start();
        }

        public void UpdateCanvas(Canvas canvas)
        {
            if (canvas != null)
            {
                _queue.Enqueue(canvas);
                _signal.Set();
            }
        }

        private void _OnUpdate(object sender, ElapsedEventArgs e)
        {
            update?.Invoke();
        }

        private void convertApplicationCanvasToCanvas(Canvas canvas)
        {
            for(int x = 0; x < canvas.Width; x++)
            {
                for (int y = 0; y < canvas.Height; y++)
                {
                    var pixel = canvas.GetPixel(x, y);
                    if (pixel != null)
                        if (x + canvas.X < this.canvas.Width && y + canvas.Y < this.canvas.Height)
                            this.canvas.SetPixel(x + canvas.X, y + canvas.Y, new Color(pixel.R, pixel.G, pixel.B));
                }
            }
        }

        private void Render()
        {
            while(true)
            {
                _signal.WaitOne(60);

                if (!NoRender)
                {
                    _queue.TryDequeue(out Canvas item);
                    if (item != null)
                        convertApplicationCanvasToCanvas(item);
                    matrix.SwapOnVsync(canvas);
                    canvas.Clear();
                }
            }
        }
    }
}
