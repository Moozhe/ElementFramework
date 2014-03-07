using System;
using System.Collections.Generic;
using System.Text;
using ElementFramework;
using System.Drawing;
using System.Windows.Forms;

namespace ElementFrameworkTest
{
    public class TestContainer : ElementContainer
    {
        public TestContainer()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);
        }
    }
}
