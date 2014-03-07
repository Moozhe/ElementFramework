using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementFramework.Primitives
{
    public class ScrollBarElement : UIElement
    {
        public bool IsHorizontal { get; set; }

        private ButtonElement decrementButton;
        private ButtonElement incrementButton;
        private UIElement trackBar;
        private UIElement thumb;

        public ScrollBarElement()
        {
            decrementButton = new ButtonElement { Name = "decrementButton", Dock = DockStyle.Top };
            incrementButton = new ButtonElement { Name = "incrementButton", Dock = DockStyle.Bottom };
            trackBar = new UIElement { Name = "trackBar", Dock = DockStyle.Fill };
            thumb = new UIElement { Name = "thumb" };

            decrementButton.Render += decrementButton_Render;
            incrementButton.Render += incrementButton_Render;
            trackBar.Render += trackBar_Render;
            thumb.Render += thumb_Render;

            trackBar.Layout += trackBar_Layout;

            Children.Add(decrementButton);
            Children.Add(incrementButton);
            Children.Add(trackBar);

            trackBar.Children.Add(thumb);
        }

        protected override void OnLayout(LayoutChangedEventArgs e)
        {
            base.OnLayout(e);

        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);


        }

        //private void DrawVerticalScrollBar(RenderEventArgs e)
        //{
        //    RectangleF rect = new RectangleF(ClientRectangle.Right - VerticalScrollBarWidth, ClientRectangle.Top, VerticalScrollBarWidth, ClientRectangle.Height);

        //    RectangleF topButtonRect = new RectangleF(rect.Left, rect.Top, VerticalScrollBarWidth, VerticalScrollBarWidth);
        //    RectangleF bottomButtonRect = new RectangleF(rect.Left, rect.Bottom - VerticalScrollBarWidth, VerticalScrollBarWidth, VerticalScrollBarWidth);
        //    RectangleF barAreaRect = RectangleF.FromLTRB(rect.Left, topButtonRect.Bottom, rect.Right, bottomButtonRect.Top);

        //    if (ViewportHeight < ExtentHeight)
        //    {
        //        float scrollBarHeight = barAreaRect.Height * (ViewportHeight / ExtentHeight);
        //        float scrollStart = topButtonRect.Bottom + barAreaRect.Height * (VerticalOffset / ExtentHeight);
        //        float scrollEnd = scrollStart + scrollBarHeight;

        //        RectangleF scrollBarRect = new RectangleF(rect.Left + 1, scrollStart, rect.Width - 2, scrollEnd - scrollStart);

        //        scrollBarRect.X -= 1;
        //        scrollBarRect.Y -= 1;

        //        e.Graphics.FillRectangle(Brushes.White, scrollBarRect.X, scrollBarRect.Y, scrollBarRect.Width, scrollBarRect.Height);
        //        e.Graphics.DrawRectangle(Pens.Black, scrollBarRect.X, scrollBarRect.Y, scrollBarRect.Width, scrollBarRect.Height);
        //    }

        //    DrawVerticalButton(e, topButtonRect);
        //    DrawVerticalButton(e, bottomButtonRect);
        //}

        //private void DrawVerticalButton(RenderEventArgs e, RectangleF rect)
        //{
        //    rect.Inflate(-2, -2);
        //    rect.X -= 1;
        //    rect.Y -= 1;

        //    e.Graphics.FillEllipse(Brushes.White, rect);
        //    e.Graphics.DrawEllipse(Pens.Black, rect);
        //}

        private void decrementButton_Render(object sender, RenderEventArgs e)
        {
            
        }

        private void incrementButton_Render(object sender, RenderEventArgs e)
        {
            
        }

        private void trackBar_Render(object sender, RenderEventArgs e)
        {
            
        }

        private void thumb_Render(object sender, RenderEventArgs e)
        {
            
        }

        private void trackBar_Layout(object sender, LayoutChangedEventArgs e)
        {
            
        }
    }
}
