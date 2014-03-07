using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace ElementFramework
{
    public class BorderElement : UIElement
    {
        private UIElement contentContainer;

        private Brush _borderBrush;
        public Brush BorderBrush
        {
            get
            {
                return _borderBrush;
            }
            set
            {
                if (_borderBrush != value)
                {
                    _borderBrush = value;

                    Invalidate();
                }
            }
        }

        private float _borderThickness;
        public float BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                if (_borderThickness != value)
                {
                    _borderThickness = value;

                    contentContainer.X = value;
                    contentContainer.Y = value;
                    contentContainer.Width = ClientRectangle.Width - (2 * value);
                    contentContainer.Height = ClientRectangle.Height - (2 * value);

                    Invalidate();
                }
            }
        }

        public new UIElementCollection Children
        {
            get
            {
                return contentContainer.Children;
            }
        }

        public override bool ClipToBounds
        {
            get
            {
                return base.ClipToBounds;
            }
            set
            {
                base.ClipToBounds = value;
                contentContainer.ClipToBounds = value;
            }
        }

        public BorderElement()
        {
            contentContainer = new UIElement
            {
                Name = "contentContainer",
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
            };

            base.Children.Add(contentContainer);
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);

            if (BorderBrush != null)
            {
                using (Pen borderPen = new Pen(BorderBrush, BorderThickness))
                {
                    borderPen.LineJoin = LineJoin.Miter;

                    float margin = BorderThickness / 2;
                    RectangleF rect = ClientRectangle;

                    e.Graphics.DrawRectangle(borderPen, rect.X + margin, rect.Y + margin, rect.Width - (margin * 2), rect.Height - (margin * 2));
                }
            }
        }
    }
}
