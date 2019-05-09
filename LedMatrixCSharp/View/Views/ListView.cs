using System;
using System.Net.Sockets;
using LedMatrixCSharp.Utils;
using LedMatrixCSharp.View.Layout;

namespace LedMatrixCSharp.View.Views
{
    public class ListView: ScrollLayout
    {
        StackPanel _stackPanel  = new StackPanel();

        public int CurrentIndex { get; set; }


        private int _fixedHeight = 0;

        public int FixedHeight
        {
            get { return _fixedHeight; }
            set
            {
                _fixedHeight = value;
                Height = _fixedHeight;
            }
        }
        
        private int _fixedWidth = 0;

        public int FixedWidth
        {
            get { return _fixedWidth; }
            set
            {
                _fixedWidth = value;
                Width = _fixedWidth;
                _stackPanel.Width = _fixedWidth - 1;
            }
        }

        public void Add(View view)
        {
            _stackPanel.Add(view);
            Height = FixedHeight == 0 ? _stackPanel.Height : FixedHeight;
            Width = FixedWidth == 0 ? _stackPanel.Width + 1 : FixedWidth;
            if (FixedWidth != 0) _stackPanel.Width = _fixedWidth - 1;

            foreach (var v in _stackPanel.Items)
            {
                v.Width = _stackPanel.Width;
            }
        }
        
        public ListView(string scroller)
        {
            this.ScrollerName = scroller;

            Controls.Instance.OnScrollerScrolled(this.ScrollerName, (direction, sc) =>
            {
                Console.WriteLine(sc.Position);
                if (direction == 0 && CurrentIndex > 0)
                {
                    CurrentIndex -= 1;
                } else if (direction == 1 && CurrentIndex < _stackPanel.Count - 1)
                {
                    CurrentIndex += 1;
                }
            });
            
            Child = _stackPanel;
        }

        private View lastItem = null;

        public override void Update()
        {               
            var currentItem = _stackPanel.Get(CurrentIndex);
            
            if (lastItem == null || lastItem != currentItem)
            {
                lastItem?.Clear();
                lastItem = currentItem;
            }

            for (int x = 0; x < currentItem.Width; x++)
            {
                for (int y = 0; y < currentItem.Height; y++)
                {
                    var pixel = currentItem.GetPixel(x, y);
                    if (pixel == null)
                    {
                        currentItem.SetPixel(x, y, CanvasColor.BLUE);
                    }
                }
            }   
            
            base.Update();
        }
    }
}