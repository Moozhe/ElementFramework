using ElementFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace EmailWidgetTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(230, 230, 230), Color.FromArgb(75, 75, 75), LinearGradientMode.Vertical);

            var visualRoot = new UIElement
            {
                Name = "VisualRoot",
                Dock = DockStyle.Fill,
                Background = brush,
            };

            elementContainer.Children.Add(visualRoot);

            var widget = new EmailWidget
            {
                Name = "emailWidget",
                Bounds = new RectangleF(100, 100, 200, 200),
            };

            visualRoot.Children.Add(widget);

            widget.RotateAngle = 180f;
        }
    }
}
