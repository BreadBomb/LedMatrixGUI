using System;
using System.Collections.Generic;
using System.Text;
using LedMatrixCSharp.View.Views;
using rpi_rgb_led_matrix_sharp;

namespace LedMatrixCSharp.View.Layout
{
    class StackPanel : View
    {

        public IList<View> Children = new List<View>();
        public Orientation Orientation { get; set; } = Orientation.Vertical;

        public StackPanel(): base() { }

        public override void Draw()
        {
            int actualOffset = 0;

            Canvas.Fill(CanvasColor.GRAY);

            foreach (View child in Children)
            { 
                if (Orientation == Orientation.Vertical)
                    child.Canvas.Offset.Set(0, actualOffset);
                if (Orientation == Orientation.Horizontal)
                    child.Canvas.Offset.Set(actualOffset, 0);

                child.Draw();

                if (Orientation == Orientation.Vertical)
                    actualOffset += child.Dimensions.Height + child.Position.Y;
                if (Orientation == Orientation.Horizontal)
                    actualOffset += child.Dimensions.Width + child.Position.Y;
            }
            base.Draw();
        }
    }
}
