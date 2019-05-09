using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using LedMatrixCSharp.Utils;
using LedMatrixCSharp.View.Views;
using rpi_rgb_led_matrix_sharp;
using SixLabors.Fonts;

namespace LedMatrixCSharp.View.Layout
{
    public class StackPanel : Panel
    {
        private IList<View> Children = new List<View>();

        private Orientation _Orientation = Orientation.Vertical;
        
        public Orientation Orientation
        {
            get => _Orientation;
            set
            {
                _Orientation = value;
                CalculateDimensions();
            }
        }

        public void Add(View view)
        {
            Children.Add(view);
            CalculateDimensions();
        }

        public int Count => Children.Count;

        public View Get(int index) => Children[index];

        public IEnumerable<View> Items => Children;

        public void CalculateDimensions()
        {
            Height = 0;
            Width = 0;
            foreach (var child in Children)
            {
                if (Orientation == Orientation.Vertical)
                {
                    if (child.Width + child.X > Width)
                    {
                        Width = child.Width + child.X;
                    }
                    
                    Height += child.Height + child.Y;
                }   
                if (Orientation == Orientation.Horizontal)
                {
                    if (child.Height + child.Y > Height)
                    {
                        Height = child.Height + child.Y;
                    }

                    Width += child.Width + child.X;
                }   
            }
            Console.WriteLine($"New Dimensions. Height: {Height} Width: {Width}");
        }

        public StackPanel(): base() {
        }

        public override void Update()
        {
            Clear();
            
            int actualOffset = 0;

            foreach (View child in Children)
            {
                if (Orientation == Orientation.Vertical)
                {
                    child.OffsetY = actualOffset;
                    actualOffset += child.Height + child.Y;                    
                }
                else
                {
                    child.OffsetX = actualOffset;
                    actualOffset += child.Width + child.X;         
                }
                
                child.Update();

                Concat(child);
            }

            base.Update();
        }
    }
}
