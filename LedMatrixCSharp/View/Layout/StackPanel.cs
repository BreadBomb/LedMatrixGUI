using System;
using System.Collections.Generic;
using System.Text;
using LedMatrixCSharp.Utils;
using LedMatrixCSharp.View.Views;
using rpi_rgb_led_matrix_sharp;

namespace LedMatrixCSharp.View.Layout
{
    public class StackPanel : View
    {
        private int lastKnownChildrenCount = 0;

        private IList<View> children = new List<View>();
        public Orientation Orientation { get; set; } = Orientation.Vertical;

        public void Add(View view)
        {
            view.DrawInsideCanvas(ref Canvas);
            this.Dimensions = new Dimensions(Dimensions.Width, Dimensions.Height + view.Dimensions.Height);
            children.Add(view);
        }

        public StackPanel(): base() {
            
        }

        public override void Draw()
        {
         
            int actualOffset = 0;

            Canvas.Fill(CanvasColor.GRAY);

            View lastView = null;

            foreach (View child in children)
            {
                int testY = 0;
                if (lastView != null)
                {
                    testY = lastView.Position.Y + lastView.Dimensions.Height;
                }

                if (Orientation == Orientation.Vertical)
                    child.Position = new CanvasPosition(0, testY);
                if (Orientation == Orientation.Horizontal)
                    child.Position = new CanvasPosition(lastView.Position.X + lastView.Dimensions.Width + Position.X, 0);

                lastView = child;
                child.Draw();
            }
            base.Draw();
        }
    }
}
