using System;
using System.Collections.Generic;
using System.Text;
using LedMatrixCSharp.Utils;
using LedMatrixCSharp.View.Views;
using rpi_rgb_led_matrix_sharp;

namespace LedMatrixCSharp.View.Layout
{
    public class StackPanel : Panel
    {
        public IList<View> Children = new List<View>();
        public Orientation Orientation { get; set; } = Orientation.Vertical;


        public StackPanel(): base() {
            Width = 32;
        }

        public override void Update()
        {
            Clear();
            int actualOffset = 0;

            foreach (View child in Children)
            {
                child.OffsetY = actualOffset;
                Height = actualOffset;

                child.Update();

                Concat(child);

                actualOffset += child.Height + child.Y;
            }

            base.Update();
        }
    }
}
