using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementFramework
{
    public class ListBoxElement : UIElement
    {
        public event EventHandler ForegroundChanged;
        public event EventHandler FontFamilyChanged;
        public event EventHandler FontSizeChanged;
        public event EventHandler FontStyleChanged;
        public event EventHandler TextAlignChanged;
        public event EventHandler VerticalTextAlignChanged;

        private ScrollViewer scrollViewer;
        private StackPanel stackPanel;

        private ObservableList<object> _items = new ObservableList<object>();
        public ObservableList<object> Items
        {
            get
            {
                return _items;
            }
        }

        private Brush _foreground = Brushes.Black;
        public Brush Foreground
        {
            get
            {
                return _foreground;
            }
            set
            {
                if (_foreground != value)
                {
                    _foreground = value;

                    OnForegroundChanged(EventArgs.Empty);
                }
            }
        }

        private FontFamily _fontFamily = FontFamily.GenericSansSerif;
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                if (_fontFamily != value)
                {
                    _fontFamily = value;

                    OnFontFamilyChanged(EventArgs.Empty);
                }
            }
        }

        private float _fontSize = 12f;
        public float FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;

                    OnFontSizeChanged(EventArgs.Empty);
                }
            }
        }

        private FontStyle _fontStyle = FontStyle.Regular;
        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }
            set
            {
                if (_fontStyle != value)
                {
                    _fontStyle = value;

                    OnFontStyleChanged(EventArgs.Empty);
                }
            }
        }

        private bool _antiAliasedText = true;
        public bool AntiAliasedText
        {
            get
            {
                return _antiAliasedText;
            }
            set
            {
                if (_antiAliasedText != value)
                {
                    _antiAliasedText = value;

                    Invalidate();
                }
            }
        }

        private StringAlignment _textAlign = StringAlignment.Near;
        public StringAlignment TextAlign
        {
            get
            {
                return _textAlign;
            }
            set
            {
                if (_textAlign != value)
                {
                    _textAlign = value;

                    Invalidate();
                }
            }
        }

        private StringAlignment _verticalTextAlign = StringAlignment.Near;
        public StringAlignment VerticalTextAlign
        {
            get
            {
                return _verticalTextAlign;
            }
            set
            {
                if (_verticalTextAlign != value)
                {
                    _verticalTextAlign = value;

                    OnVerticalTextAlignChanged(EventArgs.Empty);
                }
            }
        }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                return scrollViewer.VerticalScrollBarVisibility;
            }
            set
            {
                scrollViewer.VerticalScrollBarVisibility = value;
            }
        }

        public ScrollBarVisibility HorizontalScrollBarVisiblity
        {
            get
            {
                return scrollViewer.HorizontalScrollBarVisibility;
            }
            set
            {
                scrollViewer.HorizontalScrollBarVisibility = value;
            }
        }

        private ElementFactory _itemTemplate;
        public ElementFactory ItemTemplate
        {
            get
            {
                return _itemTemplate;
            }
            set
            {
                if (_itemTemplate != value)
                {
                    _itemTemplate = value;
                }
            }
        }

        public ListBoxElement()
        {
            ItemTemplate = new ListBoxItemFactory { ListBox = this };

            scrollViewer = new ScrollViewer
            {
                Name = "scrollViewer",
                Dock = DockStyle.Fill,
            };

            this.Children.Add(scrollViewer);

            stackPanel = new StackPanel
            {
                Name = "stackPanel",
                Orientation = Orientation.Vertical,
            };

            scrollViewer.Children.Add(stackPanel);

            Items.CollectionChanged += Items_CollectionChanged;

            // TODO
            BoundsChanged += (s, e) => stackPanel.Width = scrollViewer.ClientRectangle.Width;
        }

        protected virtual void OnForegroundChanged(EventArgs e)
        {
            Invalidate();

            if (ForegroundChanged != null)
                ForegroundChanged(this, e);
        }

        protected virtual void OnFontFamilyChanged(EventArgs e)
        {
            Invalidate();

            if (FontFamilyChanged != null)
                FontFamilyChanged(this, e);
        }

        protected virtual void OnFontSizeChanged(EventArgs e)
        {
            Invalidate();

            if (FontSizeChanged != null)
                FontSizeChanged(this, e);
        }

        protected virtual void OnFontStyleChanged(EventArgs e)
        {
            Invalidate();

            if (FontStyleChanged != null)
                FontStyleChanged(this, e);
        }

        protected virtual void OnTextAlignChanged(EventArgs e)
        {
            Invalidate();

            if (TextAlignChanged != null)
                TextAlignChanged(this, e);
        }

        protected virtual void OnVerticalTextAlignChanged(EventArgs e)
        {
            Invalidate();

            if (VerticalTextAlignChanged != null)
                VerticalTextAlignChanged(this, e);
        }

        private void Items_CollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            foreach (object item in e.ItemsRemoved)
            {
                stackPanel.Children.Remove(stackPanel.Children[item]);
            }

            foreach (object item in e.ItemsAdded)
            {
                stackPanel.Children.Add(ItemTemplate.Create(item));
            }
        }

        public class ListBoxItem : UIElement
        {
            public ListBoxElement Owner { get; internal set; }

            public ListBoxItem()
            {
            }

            public override SizeF GetDesiredSize(SizeF availableSize)
            {
                if (Container != null)
                {
                    using (Font font = new Font(Owner.FontFamily, Owner.FontSize, Owner.FontStyle, GraphicsUnit.Pixel))
                    using (Graphics g = (Container.CreateGraphics()))
                    {
                        return g.MeasureString(this.ToString(), font);
                    }
                }

                return base.GetDesiredSize(availableSize);
            }

            protected override void OnRender(RenderEventArgs e)
            {
                base.OnRender(e);

                if (Owner.Foreground != null)
                {
                    if (Owner.AntiAliasedText)
                        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    using (Font font = new Font(Owner.FontFamily, Owner.FontSize, Owner.FontStyle, GraphicsUnit.Pixel))
                    using (StringFormat stringFormat = new StringFormat { Alignment = Owner.TextAlign, LineAlignment = Owner.VerticalTextAlign })
                        e.Graphics.DrawString(this.ToString(), font, Owner.Foreground, ClientRectangle, stringFormat);
                }
            }

            public override string ToString()
            {
                return Tag.ToString();
            }
        }

        private class ListBoxItemFactory : ElementFactory
        {
            public ListBoxElement ListBox { get; internal set; }

            public override UIElement Create(object obj)
            {
                return new ListBoxItem
                {
                    Owner = ListBox,
                    Tag = obj,
                };
            }
        }
    }
}
