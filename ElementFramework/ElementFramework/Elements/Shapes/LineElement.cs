using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    public class LineElement : ShapeElement
    {
        public LineElement()
        {
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);

            Rectangle rect = Utility.RoundRectangle(ClientRectangle);
            rect.Width -= 1;
            rect.Height -= 1;

            //rect.X += StrokeThickness / 2;
            //rect.Y += StrokeThickness / 2;
            //rect.Width -= StrokeThickness;
            //rect.Height -= StrokeThickness;

            //if (Fill != null)
            //{
            //    e.Graphics.FillEllipse(Fill, rect.X, rect.Y, rect.Width, rect.Height);
            //}

            //if (Stroke != null)
            //{
            //    using (Pen pen = new Pen(Stroke, StrokeThickness))
            //    {
            //        pen.Alignment = PenAlignment.Inset;

            //        e.Graphics.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
            //    }
            //}

            if (Stroke != null)
            {
                using (Pen pen = new Pen(Stroke, StrokeThickness))
                {
                    e.Graphics.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Bottom);
                }
            }
        }
    }
}
