using LedMatrixCSharp.Utils;
using System;

namespace LedMatrixCSharp.View.Layout
{
    public class ScrollLayout: Panel
    {
        private View child;
        private int yOffset;
        
        public string ScrollerName { get; set; }
        
        public View Child
        {
            get { return child; }
            set
            {
                child = value;
                if (Width >= 32)
                {
                    child.Width = 31;
                }

                Width = 32;
                Height = child.Height;
            }
        }

        //State of Pin08
        private static bool levA = false;
        //State of Pin7
        private static bool levB = false;
        //The name of the last GPIO pin to fire a PinStatusChanged event
        private static string lastGpio = String.Empty;

        public ScrollLayout(string scroller)
        {
            this.ScrollerName = scroller;

            Controls.Instance.OnScrollerScrolled(this.ScrollerName, (direction, sc) =>
            {
                if (direction == 0)
                {
                    yOffset--;
                } else
                {
                    yOffset++;
                }
            });
        }
        
        public override void Update()
        {
            Clear();
            SetPixel(31, 0, CanvasColor.YELLOW);
            SetPixel(31, 31, CanvasColor.YELLOW);
            for (int i = 0; i < (30d / Height)*30d+1; i++)
            {
                SetPixel(31, i+1+(int)Math.Round(((double)-yOffset / (double)(Height)) * 32, 0), CanvasColor.WHITE);
            }

            if (Child != null)
            {
                Child.Y  = yOffset;
            }

            Child?.Update();
            
            Concat(Child);

            base.Update();
        }
    }
}