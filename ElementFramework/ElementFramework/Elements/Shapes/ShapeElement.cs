using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    public abstract class ShapeElement : UIElement
    {
        public event EventHandler FillChanged;
        public event EventHandler StrokeChanged;
        public event EventHandler StrokeThicknessChanged;

        private Brush _fill;
        public Brush Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                if (_fill != value)
                {
                    _fill = value;

                    OnFillChanged(EventArgs.Empty);
                }
            }
        }

        private Brush _stroke;
        public Brush Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                if (_stroke != value)
                {
                    _stroke = value;

                    OnStrokeChanged(EventArgs.Empty);
                }
            }
        }

        private float _strokeThickness = 1f;
        public float StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                if (_strokeThickness != value)
                {
                    _strokeThickness = value;

                    OnStrokeThicknessChanged(EventArgs.Empty);
                }
            }
        }

        private bool _antiAliased = false;
        public bool AntiAliased
        {
            get
            {
                return _antiAliased;
            }
            set
            {
                _antiAliased = value;
            }
        }

        public ShapeElement()
        {
        }

        protected override void OnRender(RenderEventArgs e)
        {
            if (AntiAliased)
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            base.OnRender(e);
        }

        protected virtual void OnFillChanged(EventArgs e)
        {
            Invalidate();

            if (FillChanged != null)
                FillChanged(this, e);
        }

        protected virtual void OnStrokeChanged(EventArgs e)
        {
            Invalidate();

            if (StrokeChanged != null)
                StrokeChanged(this, e);
        }

        protected virtual void OnStrokeThicknessChanged(EventArgs e)
        {
            Invalidate();

            if (StrokeThicknessChanged != null)
                StrokeThicknessChanged(this, e);
        }
    }
}
