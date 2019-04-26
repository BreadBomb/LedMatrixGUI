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

            StackPanel stackPanel = new StackPanel();

            Image image = new Image("Animations/7.bmp");
            image.Dimensions = new Dimensions(16, 16);
            image.Position = new CanvasPosition(8, 5);

            Label label = new Label("Tropfen");
            label.Position.X = 2;

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(label);

            matrixApplication.Child = stackPanel;
        }
    }
}
