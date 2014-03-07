using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    public class RectangleElement : ShapeElement
    {
        public RectangleElement()
        {
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);

            RectangleF rect = ClientRectangle;
            float margin = StrokeThickness / 2;

            if (Fill != null)
            {
                e.Graphics.FillRectangle(Fill, rect.X + margin, rect.Y + margin, rect.Width - (margin * 2), rect.Height - (margin * 2));
            }

            if (Stroke != null)
            {
                using (Pen pen = new Pen(Stroke, StrokeThickness))
                {
                    pen.LineJoin = LineJoin.Miter;

                    e.Graphics.DrawRectangle(pen, rect.X + margin, rect.Y + margin, rect.Width - (margin * 2), rect.Height - (margin * 2));
                }
            }
        }
    }
}
