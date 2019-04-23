using System;
using System.Linq;
using System.Runtime.InteropServices;
using LedMatrixCSharp.Utils;
using LedMatrixCSharp.View;
using LedMatrixCSharp.View.Layout;
using LedMatrixCSharp.View.Views;
using rpi_rgb_led_matrix_sharp;

namespace LedMatrixCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Matrix is starting...");

            MatrixApplication matrixApplication;

            if (args.Contains("--norender"))
            {
                matrixApplication = new MatrixApplication(true);
            } else
            {
                matrixApplication = new MatrixApplication();
            }

            Rectangle rectangle = new Rectangle(0, 0, 10, 15, new CanvasColor(255, 255, 255));
            Label label = new Label();

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(rectangle);
            stackPanel.Children.Add(label);

            matrixApplication.Child = stackPanel;

            Random r = new Random();

            while (true)
            {
                if (stackPanel.Orientation == Orientation.Vertical)
                {
                    stackPanel.Orientation = Orientation.Horizontal;
                } else
                {
                    stackPanel.Orientation = Orientation.Vertical;
                }
                Console.ReadKey();
            }
        }
    }
}
