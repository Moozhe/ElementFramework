using BLUE.Mail.DAO;
using ElementFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace EmailWidgetTest
{
    public class EmailWidget : UIElement
    {
        private BorderElement border;
        private ThumbElement titleThumb;
        private ListBoxElement emailListBox;

        private ImapClient client;

        public string Host { get; set; }
        public int Port { get; set; }
        public NetworkCredential Credentials { get; set; }

        public EmailWidget()
        {
            border = new BorderElement
            {
                Name = "border",
                Dock = DockStyle.Fill,
                BorderThickness = 2f,
                BorderBrush = Brushes.Black,
                Background = Brushes.White,
            };

            this.Children.Add(border);

            titleThumb = new ThumbElement
            {
                Name = "titleThumb",
                Dock = DockStyle.Top,
                Height = 20,
                Background = Brushes.CornflowerBlue,
            };

            titleThumb.DragDelta += titleThumb_DragDelta;
            border.Children.Add(titleThumb);

            var titleText = new TextElement
            {
                Name = "titleText",
                Text = "Email Widget",
                Dock = DockStyle.Fill,
                Foreground = Brushes.Black,
                TextAlign = StringAlignment.Center,
                VerticalTextAlign = StringAlignment.Center,
                FontSize = 10f,
                FontFamily = new FontFamily("Arial"),
            };

            titleThumb.Children.Add(titleText);

            var refreshButton = new ButtonElement
            {
                Name = "refreshButton",
                Size = new SizeF(80, 17),
                Dock = DockStyle.Bottom,
                Background = Brushes.CornflowerBlue,
            };

            refreshButton.IsPressedChanged += (s, e) =>
                {
                    if (refreshButton.IsPressed)
                        refreshButton.Background = Brushes.Cyan;
                    else
                        refreshButton.Background = Brushes.CornflowerBlue;
                };

            refreshButton.Click += (s, e) => GetMessages();

            var refreshText = new TextElement
            {
                Foreground = Brushes.Black,
                Text = "Refresh",
                TextAlign = StringAlignment.Center,
                VerticalTextAlign = StringAlignment.Center,
                Dock = DockStyle.Fill,
                FontSize = 10f,
            };

            refreshButton.Children.Add(refreshText);
            border.Children.Add(refreshButton);

            emailListBox = new ListBoxElement
            {
                Name = "emailListBox",
                Dock = DockStyle.Fill,
                Background = Brushes.SeaShell,
            };

            border.Children.Add(emailListBox);
        }

        protected override void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            if (client == null)
                client = new ImapClient();
        }

        public void Connect()
        {
            try
            {
                client.Connect(OnConnect, Host, Port, true, false);
            }
            catch (Exception ex)
            {
                // TODO
            }
        }

        private void Login()
        {
            client.Login(OnLogin, Credentials.UserName, Credentials.Password);
        }

        private void GetMessages()
        {
            if (client.State != ConnectionState.Transaction)
                return;

            emailListBox.Items.Clear();

            client.GetMessageCount(
                (count, ex) =>
                {
                    if (ex != null)
                    {
                        MessageBox.Show(Container, ex.ToString(), "Get Messages Failed", MessageBoxButtons.OK);
                        return;
                    }

                    client.GetMessages(
                        (messages, ex2) =>
                        {
                            if (ex2 != null)
                            {
                                MessageBox.Show(Container, ex2.ToString(), "Get Messages Failed", MessageBoxButtons.OK);
                                return;
                            }

                            emailListBox.Items.Clear();

                            for (int i = messages.Length - 1; i >= 0; i--)
                                emailListBox.Items.Add(messages[i]);
                        },
                        0, count - 1);
                });
        }

        private void OnConnect(Exception exception)
        {
            if (exception != null)
            {
                MessageBox.Show(Container, exception.ToString(), "Connect Failed", MessageBoxButtons.OK);
                return;
            }

            Login();
        }

        private void OnLogin(Exception exception)
        {
            if (exception != null)
            {
                MessageBox.Show(Container, exception.ToString(), "Login Failed", MessageBoxButtons.OK);
                return;
            }

            GetMessages();
        }

        private void titleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            this.SuspendLayout();
            this.X += e.HorizontalChange;
            this.Y += e.VerticalChange;
            this.ResumeLayout();
        }
    }
}
