using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ElementFramework
{
    public class PageElement : UIElement
    {
        public PageElement()
        {
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);

            e.Graphics.FillRectangle(Brushes.White, ClientRectangle);

            //for (int x = 100; x <= Width; x += 100)
            //{
            //    e.Graphics.DrawLine(Pens.Black, new PointF(x, 0), new PointF(x, Height));
            //}

            //for (int y = 100; y <= Height; y += 100)
            //{
            //    e.Graphics.DrawLine(Pens.Black, new PointF(0, y), new PointF(Width, y));
            //}
        }

        protected override void OnMouseDown(MouseRoutedEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseRoutedEventArgs e)
        {
            base.OnMouseUp(e);
        }
    }
}
