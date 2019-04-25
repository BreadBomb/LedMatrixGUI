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

            Image image = new Image("Animations/7.bmp");

            matrixApplication.Child = image;

            Random r = new Random();
        }
    }
}
