using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public class ButtonElement : ButtonBase
    {
        public ButtonElement()
        {
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);
        }

        protected override void OnMouseDown(MouseRoutedEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseRoutedEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseRoutedEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }
    }
}
