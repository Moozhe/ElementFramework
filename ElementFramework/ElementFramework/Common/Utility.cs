using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    internal static class Utility
    {
        public static Rectangle RoundRectangle(RectangleF rect)
        {
            return Rectangle.FromLTRB(
                (int)Math.Floor(rect.Left),
                (int)Math.Floor(rect.Top),
                (int)Math.Ceiling(rect.Right),
                (int)Math.Ceiling(rect.Bottom)
            );
        }

        public static PointF TransformPoint(Matrix matrix, PointF point)
        {
            PointF[] buffer = new PointF[] { point };
            matrix.TransformPoints(buffer);
            return buffer[0];
        }

        public static void FillRectangle(Graphics graphics, Brush brush, RectangleF rectangle)
        {
            graphics.FillRectangle(brush, rectangle);

            using (Pen pen = new Pen(brush, 1.0f))
            {
                RectangleF rect = new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width - 1, rectangle.Height - 1);

                graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }
    }
}
