using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using ElementFramework;
using EmailWidgetTest;

namespace ElementFrameworkTest
{
    public partial class TestForm : Form
    {
        private PageElement page;
        private UIElement pageBorder;

        private UIElement previousItem;

        private UIElement _selectedItem;
        public UIElement SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    previousItem = _selectedItem;
                    _selectedItem = value;

                    UpdateTransformControls();
                }
            }
        }

        public TestForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            elementContainer.MouseWheel += testContainer_MouseWheel;

            var workspace = new UIElement();
            workspace.Name = "workspace";
            workspace.Dock = DockStyle.Fill;
            elementContainer.Children.Add(workspace);

            pageBorder = new UIElement();
            pageBorder.Name = "Border";
            pageBorder.ClipToBounds = true;
            pageBorder.Render += new RenderEventHandler(pageBorder_Render);
            pageBorder.Bounds = new RectangleF(50, 50, workspace.ClientRectangle.Width - 100, workspace.ClientRectangle.Height - 100);
            pageBorder.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            workspace.Children.Add(pageBorder);

            page = new PageElement();
            page.Name = "Page";
            page.Background = Brushes.White;
            page.ClipToBounds = true;
            page.Bounds = new RectangleF(2, 2, pageBorder.ClientRectangle.Width - 4, pageBorder.ClientRectangle.Height - 4);
            page.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            pageBorder.Children.Add(page);

            ScrollViewer viewer = new ScrollViewer();
            viewer.Name = "scrollViewer";
            viewer.Dock = DockStyle.Fill;
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

            page.Children.Add(viewer);

            var emailWidget = new EmailWidget
            {
                Name = "emailWidget",
                Bounds = new RectangleF(100, 50, 300, 300),
            };

            viewer.Children.Add(emailWidget);
        }

        private void UpdateTransformControls()
        {
            translateX_txt.Enabled = name_txt.Enabled = type_txt.Enabled = translateY_txt.Enabled = scaleX_txt.Enabled = scaleY_txt.Enabled = rotateAngle_txt.Enabled =
                rotateOriginX_txt.Enabled = rotateOriginY_txt.Enabled = SelectedItem != null;

            selectParent_btn.Enabled = SelectedItem != null && SelectedItem.Parent != null;

            selectPrevious_btn.Enabled = previousItem != null;

            if (SelectedItem != null)
            {
                name_txt.Text = SelectedItem.Name;
                type_txt.Text = SelectedItem.GetType().ToString();
                translateX_txt.Text = SelectedItem.TranslateX.ToString();
                translateY_txt.Text = SelectedItem.TranslateY.ToString();
                scaleX_txt.Text = SelectedItem.ScaleX.ToString();
                scaleY_txt.Text = SelectedItem.ScaleY.ToString();
                rotateAngle_txt.Text = SelectedItem.RotateAngle.ToString();
                rotateOriginX_txt.Text = SelectedItem.RotateOrigin.X.ToString();
                rotateOriginY_txt.Text = SelectedItem.RotateOrigin.Y.ToString();
            }
        }
        
        private void pageBorder_Render(object sender, RenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(Brushes.Black, 2.0f))
            {
                pen.Alignment = PenAlignment.Inset;

                e.Graphics.DrawRectangle(pen, 0, 0, pageBorder.Width, pageBorder.Height);
            }
        }

        private void testContainer_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedItem = elementContainer.GetChildAt(e.Location);
        }

        private void testContainer_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void testContainer_MouseWheel(object sender, MouseEventArgs e)
        {
            pageBorder.RotateAngle += (e.Delta > 0 ? 15 : -15);
        }

        private void testContainer_SizeChanged(object sender, EventArgs e)
        {
        }

        private void translateX_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(translateX_txt.Text, out num))
                SelectedItem.TranslateX = num;
        }

        private void translateY_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(translateY_txt.Text, out num))
                SelectedItem.TranslateY = num;
        }

        private void scaleX_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(scaleX_txt.Text, out num) && num != 0)
                SelectedItem.ScaleX = num;
        }

        private void scaleY_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(scaleY_txt.Text, out num) && num != 0)
                SelectedItem.ScaleY = num;
        }

        private void rotateAngle_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(rotateAngle_txt.Text, out num))
                SelectedItem.RotateAngle = num;
        }

        private void rotateOriginX_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(rotateOriginX_txt.Text, out num))
                SelectedItem.RotateOrigin = new PointF(num, SelectedItem.RotateOrigin.Y);
        }

        private void rotateOriginY_txt_TextChanged(object sender, EventArgs e)
        {
            float num;

            if (float.TryParse(rotateOriginY_txt.Text, out num))
                SelectedItem.RotateOrigin = new PointF(SelectedItem.RotateOrigin.X, num);
        }

        private void selectParent_btn_Click(object sender, EventArgs e)
        {
            if (SelectedItem.Parent != null)
                SelectedItem = SelectedItem.Parent;
        }

        private void selectPrevious_btn_Click(object sender, EventArgs e)
        {
            SelectedItem = previousItem;
        }
    }
}