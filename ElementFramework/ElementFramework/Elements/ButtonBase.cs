using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public class ButtonBase : UIElement
    {
        public event EventHandler Click;
        public event EventHandler IsPressedChanged;

        private bool _isPressed;
        public bool IsPressed
        {
            get
            {
                return _isPressed;
            }
            private set
            {
                if (_isPressed != value)
                {
                    _isPressed = value;

                    OnIsPressedChanged(EventArgs.Empty);
                }
            }
        }

        public ButtonBase()
        {
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);
        }

        protected override void OnMouseDown(MouseRoutedEventArgs e)
        {
            base.OnMouseDown(e);

            IsPressed = true;

            CaptureMouse();

            Invalidate();
        }

        protected override void OnMouseUp(MouseRoutedEventArgs e)
        {
            base.OnMouseUp(e);

            IsPressed = false;

            ReleaseMouseCapture();

            if (ClientRectangle.Contains(e.GetPosition(this)))
                OnClick(EventArgs.Empty);

            Invalidate();
        }

        protected override void OnMouseMove(MouseRoutedEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            Invalidate();
        }

        protected virtual void OnIsPressedChanged(EventArgs e)
        {
            if (IsPressedChanged != null)
                IsPressedChanged(this, e);
        }

        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
    }
}
