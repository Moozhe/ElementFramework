using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace ElementFramework
{
    public class TextElement : UIElement
    {
        public event EventHandler TextChanged;
        public event EventHandler ForegroundChanged;
        public event EventHandler FontFamilyChanged;
        public event EventHandler FontSizeChanged;
        public event EventHandler FontStyleChanged;
        public event EventHandler TextAlignChanged;
        public event EventHandler VerticalTextAlignChanged;

        private Brush _foreground;
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
                _antiAliasedText = value;
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;

                    OnTextChanged(EventArgs.Empty);
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

                    OnTextAlignChanged(EventArgs.Empty);
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

        public TextElement()
        {

        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);

            if (Background != null)
            {
                e.Graphics.FillRectangle(Background, ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            }

            if (Text != null && Foreground != null)
            {
                using (StringFormat stringFormat = new StringFormat())
                using (Font font = new Font(FontFamily ?? FontFamily.GenericSansSerif, FontSize))
                {
                    stringFormat.Alignment = TextAlign;
                    stringFormat.LineAlignment = VerticalTextAlign;

                    if (AntiAliasedText)
                        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    e.Graphics.DrawString(Text, font, Foreground, ClientRectangle, stringFormat);
                }
            }
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

        protected virtual void OnTextChanged(EventArgs e)
        {
            Invalidate();

            if (TextChanged != null)
                TextChanged(this, e);
        }
    }
}
