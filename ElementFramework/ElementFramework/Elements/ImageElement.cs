using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    public class ImageElement : UIElement
    {
        public event EventHandler ImageChanged;

        private Image _image;
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image != value)
                {
                    _image = value;

                    OnImageChanged(EventArgs.Empty);

                    if (ImageChanged != null)
                        ImageChanged(this, EventArgs.Empty);
                }
            }
        }

        public ImageElement()
        {

        }

        //private float[][] invertMatrix = new float[][]
        //{
        //    new float[] { -1.0f,  0.0f,  0.0f,  0.0f,  0.0f },
        //    new float[] {  0.0f, -1.0f,  0.0f,  0.0f,  0.0f },
        //    new float[] {  0.0f,  0.0f, -1.0f,  0.0f,  0.0f },
        //    new float[] {  0.0f,  0.0f,  0.0f,  1.0f,  0.0f },
        //    new float[] {  1.0f,  1.0f,  1.0f,  0.0f,  1.0f },
        //};

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);

            e.Graphics.DrawImage(Image, new RectangleF(0, 0, Width, Height));

            //using (Bitmap buffer = new Bitmap((int)Width, (int)Height))
            //{
            //    using (Graphics bg = Graphics.FromImage(buffer))
            //    {
            //        bg.DrawImage(Image, new RectangleF(0, 0, Width, Height));
            //    }

            //    using (ImageAttributes imageAttributes = new ImageAttributes())
            //    {
            //        ColorMatrix colorMatrix = new ColorMatrix(invertMatrix);
            //        imageAttributes.SetColorMatrix(colorMatrix);

            //        using (TextureBrush brush = new TextureBrush(buffer, new Rectangle(0, 0, buffer.Width, buffer.Height), imageAttributes))
            //        using (Pen pen = new Pen(brush, 2.0f))
            //        {
            //            g.DrawImage(Image, 0, 0, Width, Height);
            //            g.DrawLine(pen, 0, Height / 2, Width, Height / 2);
            //        }
            //    }
            //}
        }

        protected virtual void OnImageChanged(EventArgs e)
        {
            Invalidate();
        }
    }
}
