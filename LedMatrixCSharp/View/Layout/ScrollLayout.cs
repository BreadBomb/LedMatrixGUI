using LedMatrixCSharp.Utils;
using System;
using System.ComponentModel;
using System.Data;

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

                Width = child.Width +1;
                Height = child.Height;
            }
        }

        public ScrollLayout() {}

        public ScrollLayout(string scroller)
        {
            this.ScrollerName = scroller;

            Controls.Instance.OnScrollerScrolled(this.ScrollerName, (direction, sc) =>
            {
                Console.WriteLine(yOffset);
                if (direction == 0 && yOffset > 0)
                {
                    yOffset--;
                }
                else if (direction == 1 && yOffset < Height - 32)
                {
                    yOffset++;
                }
            });
        }

        public override void Update()
        {
            var containerHeight = (Height < 32 ? Height : 32);
            Clear();
            SetPixel(Width-1, 0, CanvasColor.YELLOW);
            SetPixel(Width-1, containerHeight - 1, CanvasColor.YELLOW);
            for (int i = 0; i < ((containerHeight - 2d) / Height)*(containerHeight - 2d); i++)
            {
                SetPixel(Width-1, i+1+(int)Math.Round(((double)yOffset / (double)(Height)) * containerHeight, 0), CanvasColor.WHITE);
            }

            if (Child != null)
            {
                Child.OffsetY  = -yOffset;
            }

            Child?.Update();
            
            Concat(Child);

            base.Update();
        }
    }
}