using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ElementFramework
{
    public delegate void RenderEventHandler(object sender, RenderEventArgs e);

    public class UIElement
    {
        public event EventHandler ParentChanged;
        public event EventHandler<CollectionChangedEventArgs> ChildrenChanged;
        public event EventHandler IsVisibleChanged;
        public event EventHandler BackgroundChanged;
        public event RenderEventHandler Render;
        public event EventHandler<LayoutChangedEventArgs> Layout;
        public event EventHandler BoundsChanged;
        public event EventHandler<MouseRoutedEventArgs> MouseDown;
        public event EventHandler<MouseRoutedEventArgs> MouseUp;
        public event EventHandler<MouseRoutedEventArgs> MouseMove;
        public event EventHandler Enter;
        public event EventHandler Leave;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler Loaded;

        private bool layoutSuspended;
        private RectangleF suspendedBounds;

        private ElementContainer _container;
        public ElementContainer Container
        {
            get
            {
                return _container;
            }
            internal set
            {
                _container = value;

                Children.Container = value;

                OnLoaded(EventArgs.Empty);
            }
        }

        private UIElementCollection _children;
        public virtual UIElementCollection Children
        {
            get
            {
                return _children;
            }
            private set
            {
                _children = value;
            }
        }

        private UIElement _parent;
        public UIElement Parent
        {
            get
            {
                return _parent;
            }
            protected internal set
            {
                if (_parent != value)
                {
                    _parent = value;

                    OnParentChanged(EventArgs.Empty);
                }
            }
        }

        private string _name = String.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private object _tag;
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (_tag != value)
                {
                    _tag = value;
                }
            }
        }

        private Brush _background;
        public Brush Background
        {
            get
            {
                return _background;
            }
            set
            {
                if (_background != value)
                {
                    _background = value;

                    OnBackgroundChanged(EventArgs.Empty);
                }
            }
        }

        private float _x;
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                if (X != value)
                    Bounds = new RectangleF(value, Y, Width, Height);
            }
        }

        private float _y;
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (Y != value)
                    Bounds = new RectangleF(X, value, Width, Height);
            }
        }

        private float _width;
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (Width != value)
                    Bounds = new RectangleF(X, Y, value, Height);
            }
        }

        private float _height;
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (Height != value)
                    Bounds = new RectangleF(X, Y, Width, value);
            }
        }

        public PointF Location
        {
            get
            {
                return new PointF(X, Y);
            }
            set
            {
                if (Location != value)
                    Bounds = new RectangleF(value.X, value.Y, Width, Height);
            }
        }

        public SizeF Size
        {
            get
            {
                return new SizeF(Width, Height);
            }
            set
            {
                if (Size != value)
                    Bounds = new RectangleF(X, Y, value.Width, value.Height);
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X, Y, Width, Height);
            }
            set
            {
                if (Bounds != value)
                {
                    Invalidate();

                    RectangleF oldBounds = Bounds;

                    _x = value.X;
                    _y = value.Y;
                    _width = value.Width;
                    _height = value.Height;

                    PerformLayout(new LayoutChangedEventArgs(Bounds, oldBounds));

                    Invalidate();

                    OnBoundsChanged(EventArgs.Empty);
                }
            }
        }

        public float Left
        {
            get
            {
                return X;
            }
        }

        public float Top
        {
            get
            {
                return Y;
            }
        }

        public float Right
        {
            get
            {
                return X + Width;
            }
        }

        public float Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        public virtual RectangleF ClientRectangle
        {
            get
            {
                return new RectangleF(0, 0, Width, Height);
            }
        }

        private bool _isVisible = true;
        public virtual bool IsVisible
        {
            get
            {
                if (!_isVisible)
                    return false;

                UIElement parent = this.Parent;

                while (parent != null)
                {
                    if (!parent.IsVisible)
                        return false;

                    parent = parent.Parent;
                }

                return _isVisible;
            }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;

                    OnIsVisibleChanged(EventArgs.Empty);
                }
            }
        }

        private HorizontalAlignment _horizontalAlignment;
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _horizontalAlignment;
            }
            set
            {
                if (_horizontalAlignment != value)
                {
                    _horizontalAlignment = value;

                    InvalidateMeasure();
                }
            }
        }

        private HorizontalAlignment _verticalAlignment;
        public HorizontalAlignment VerticalAlignment
        {
            get
            {
                return _verticalAlignment;
            }
            set
            {
                if (_verticalAlignment != value)
                {
                    _verticalAlignment = value;

                    InvalidateMeasure();
                }
            }
        }

        private AnchorStyles _anchor;
        public AnchorStyles Anchor
        {
            get
            {
                return _anchor;
            }
            set
            {
                if (_anchor != value)
                {
                    _anchor = value;

                    if (_anchor != AnchorStyles.None)
                        _dock = DockStyle.None;

                    InvalidateMeasure();
                }
            }
        }

        private DockStyle _dock;
        public DockStyle Dock
        {
            get
            {
                return _dock;
            }
            set
            {
                if (_dock != value)
                {
                    _dock = value;

                    if (_dock != DockStyle.None)
                        _anchor = AnchorStyles.None;

                    InvalidateMeasure();
                }
            }
        }

        private float _scaleX = 1.0f;
        public float ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                if (_scaleX != value)
                {
                    if (value == 0)
                        throw new ArgumentOutOfRangeException("ScaleX must be non-zero");

                    Invalidate();
                    _scaleX = value;
                    Invalidate();
                }
            }
        }

        private float _scaleY = 1.0f;
        public float ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                if (_scaleY != value)
                {
                    if (value == 0)
                        throw new ArgumentOutOfRangeException("ScaleX must be non-zero");

                    Invalidate();
                    _scaleY = value;
                    Invalidate();
                }
            }
        }

        private float _rotateAngle;
        public float RotateAngle
        {
            get
            {
                return _rotateAngle;
            }
            set
            {
                if (_rotateAngle != value)
                {
                    Invalidate();
                    _rotateAngle = value;
                    Invalidate();
                }
            }
        }

        private float _translateX;
        public float TranslateX
        {
            get
            {
                return _translateX;
            }
            set
            {
                if (_translateX != value)
                {
                    Invalidate();
                    _translateX = value;
                    Invalidate();
                }
            }
        }

        private float _translateY;
        public float TranslateY
        {
            get
            {
                return _translateY;
            }
            set
            {
                if (_translateY != value)
                {
                    Invalidate();
                    _translateY = value;
                    Invalidate();
                }
            }
        }

        private PointF _transformOrigin = new PointF(0.5f, 0.5f);
        public PointF RotateOrigin
        {
            get
            {
                return _transformOrigin;
            }
            set
            {
                if (_transformOrigin != value)
                {
                    Invalidate();
                    _transformOrigin = value;
                    Invalidate();
                }
            }
        }

        private Matrix _transform = new Matrix();
        public virtual Matrix Transform
        {
            get
            {
                _transform.Reset();

                PointF centerPoint = new PointF(Width * 0.5f, Height * 0.5f);

                _transform.Translate(TranslateX, TranslateY);
                _transform.Translate(Width * RotateOrigin.X, Height * RotateOrigin.Y);
                _transform.Rotate(RotateAngle);
                _transform.Translate(-(Width * RotateOrigin.X), -(Height * RotateOrigin.Y));
                _transform.Translate(centerPoint.X, centerPoint.Y);
                _transform.Scale(ScaleX, ScaleY);
                _transform.Translate(-(centerPoint.X), -(centerPoint.Y));

                return _transform;
            }
        }

        private bool _clipToBounds = true;
        public virtual bool ClipToBounds
        {
            get
            {
                return _clipToBounds;
            }
            set
            {
                if (_clipToBounds != value)
                {
                    _clipToBounds = value;

                    Invalidate();
                }
            }
        }

        private bool _isHitTestVisible = true;
        public virtual bool IsHitTestVisible
        {
            get
            {
                return _isHitTestVisible;
            }
            set
            {
                _isHitTestVisible = value;
            }
        }

        private bool _isFocused;
        public virtual bool IsFocused
        {
            get
            {
                return _isFocused;
            }
            internal set
            {
                if (_isFocused != value)
                {
                    _isFocused = value;

                    if (_isFocused)
                        OnEnter(EventArgs.Empty);
                    else
                        OnLeave(EventArgs.Empty);
                }
            }
        }

        private bool _isMouseOver;
        public bool IsMouseOver
        {
            get
            {
                return _isMouseOver;
            }
            internal set
            {
                if (_isMouseOver != value)
                {
                    _isMouseOver = value;

                    if (_isMouseOver)
                        OnMouseEnter(EventArgs.Empty);
                    else
                        OnMouseLeave(EventArgs.Empty);
                }
            }
        }

        public UIElement()
        {
            Children = new UIElementCollection(Container, this);
            Children.CollectionChanged += (s, e) => OnChildrenChanged(e);
        }

        public virtual SizeF GetDesiredSize(SizeF availableSize)
        {
            return new SizeF(Width, Height);
        }

        public virtual bool HitTest(PointF point)
        {
            if (IsVisible && IsHitTestVisible)
                return ClientRectangle.Contains(point);
            else
                return false;
        }

        public virtual UIElement InputHitTest(PointF point)
        {
            foreach (UIElement child in Children.GetFrontToBack())
            {
                UIElement hitTest = child.InputHitTest(PointToChild(point, child));

                if (hitTest != null)
                    return hitTest;
            }

            if (HitTest(point))
                return this;
            else
                return null;
        }

        public Matrix TransformToAncestor(UIElement ancestor)
        {
            if (!IsDescendantOf(ancestor))
                throw new ArgumentException("The specified element is not an ancestor of this element", "ancestor");

            Matrix matrix = new Matrix();
            UIElement element = this;

            while (element != ancestor)
            {
                matrix.Translate(element.X, element.Y);
                matrix.Multiply(element.Transform);


                element = element.Parent;
            }

            return matrix;
        }

        public Matrix TransformToDescendant(UIElement descendant)
        {
            if (!IsAncestorOf(descendant))
                throw new ArgumentException("The specified element is not a descendant of this element", "descendant");

            Matrix matrix = descendant.TransformToAncestor(this);

            matrix.Invert();

            return matrix;
        }

        public Matrix TransformToElement(UIElement element)
        {
            UIElement ancestor = FindCommonAncestor(element);
            Matrix transform = TransformToAncestor(ancestor);
            
            using (Matrix transformToDescendant = ancestor.TransformToDescendant(element))
                transform.Multiply(transformToDescendant);

            return transform;
        }

        public UIElement FindCommonAncestor(UIElement element)
        {
            UIElement parent = this;

            while (parent != null)
            {
                if (parent.IsAncestorOf(element))
                    return parent;

                parent = parent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Returns true if this element is an ancestor of the specified element.
        /// </summary>
        public bool IsAncestorOf(UIElement element)
        {
            UIElement parent = element.Parent;

            while (parent != null)
            {
                if (parent == this)
                    return true;

                parent = parent.Parent;
            }

            return false;
        }

        /// <summary>
        /// Returns true if this element is a descendant of the specified element.
        /// </summary>
        public bool IsDescendantOf(UIElement element)
        {
            UIElement parent = this.Parent;

            while (parent != null)
            {
                if (parent == element)
                    return true;

                parent = parent.Parent;
            }

            return false;
        }

        public PointF PointToParent(PointF point)
        {
            if (Parent != null)
                return PointToAncestor(point, Parent);
            else
                return point;
        }

        public PointF PointToChild(PointF point, UIElement child)
        {
            return PointToDescendant(point, child);
        }

        public PointF PointToAncestor(PointF point, UIElement ancestor)
        {
            using (Matrix matrix = TransformToAncestor(ancestor))
                return Utility.TransformPoint(matrix, point);
        }

        public PointF PointToDescendant(PointF point, UIElement descendant)
        {
            using (Matrix matrix = TransformToDescendant(descendant))
                return Utility.TransformPoint(matrix, point);
        }

        public PointF PointToElement(PointF point, UIElement element)
        {
            using (Matrix matrix = TransformToElement(element))
                return Utility.TransformPoint(matrix, point);
        }

        public virtual Point PointToContainer(PointF point)
        {
            UIElement parent = this.Parent;

            while (parent != null)
            {
                point = PointToParent(point);
                parent = parent.Parent;
            }

            using (Matrix matrix = parent.Transform.Clone())
            {
                matrix.Invert();
                point = Utility.TransformPoint(matrix, point);
            }

            return Point.Round(point);
        }

        public virtual Point PointToScreen(PointF point)
        {
            Point pt = PointToContainer(point);

            if (Container != null)
                pt = Container.PointToScreen(pt);

            return pt;
        }

        public virtual PointF PointFromContainer(Point containerPoint)
        {
            // TODO: Fix this logic, not working.
            UIElement element = this;
            PointF point = containerPoint;

            while (element.Parent != null)
                element = element.Parent;

            point.X -= element.X;
            point.Y -= element.Y;

            using (Matrix matrix = element.Transform.Clone())
            {
                matrix.Invert();
                point = Utility.TransformPoint(matrix, point);
            }

            if (element != this)
            {
                using (Matrix transform = element.TransformToDescendant(this))
                    point = Utility.TransformPoint(transform, point);
            }

            return point;
        }

        public virtual PointF PointFromScreen(Point screenPoint)
        {
            if (Container != null)
            {
                Point clientPoint = Container.PointToClient(screenPoint);

                return PointFromContainer(clientPoint);
            }
            else
            {
                throw new InvalidOperationException("This element is not a child of a container control.");
            }
        }

        /// <summary>
        /// Transforms the given rectangle from local coordinates to coordinates of the specified child.
        /// </summary>
        public virtual RectangleF RectToChild(RectangleF rect, UIElement child)
        {
            PointF topLeft = PointToChild(new PointF(rect.Left, rect.Top), child);
            PointF bottomRight = PointToChild(new PointF(rect.Right, rect.Bottom), child);

            return RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }

        /// <summary>
        /// Transforms the given rectangle from local coordinates to parent coordinates.
        /// </summary>
        public virtual RectangleF RectToParent(RectangleF rect)
        {
            PointF topLeft = PointToParent(new PointF(rect.Left, rect.Top));
            PointF bottomRight = PointToParent(new PointF(rect.Right, rect.Bottom));

            return RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }

        /// <summary>
        /// Transforms the given rectangle from local coordinates to client coordinates on the container control.
        /// </summary>
        public virtual Rectangle RectToContainer(RectangleF rect)
        {
            Point upperLeft = PointToContainer(new PointF(rect.Left, rect.Top));
            Point lowerRight = PointToContainer(new PointF(rect.Right, rect.Bottom));

            return Rectangle.FromLTRB(upperLeft.X, upperLeft.Y, lowerRight.X, lowerRight.Y);
        }

        public bool CaptureMouse()
        {
            if (Container != null)
                return Container.SetMouseCapture(this);

            return false;
        }

        public void ReleaseMouseCapture()
        {
            if (Container != null)
                Container.ReleaseMouseCapture(this);
        }

        public void SuspendLayout()
        {
            layoutSuspended = true;
            suspendedBounds = Bounds;
        }

        public void ResumeLayout()
        {
            layoutSuspended = false;
            PerformLayout(new LayoutChangedEventArgs(Bounds, suspendedBounds));
        }

        public void PerformLayout()
        {
            PerformLayout(new LayoutChangedEventArgs());
        }

        public void PerformLayout(LayoutChangedEventArgs e)
        {
            if (layoutSuspended)
                return;

            RectangleF dockArea = ClientRectangle;

            foreach (UIElement child in Children)
            {
                if (child.IsVisible && child.Dock != DockStyle.None)
                {
                    switch (child.Dock)
                    {
                        case DockStyle.Fill:
                            child.Bounds = dockArea;
                            dockArea = RectangleF.Empty;
                            break;

                        case DockStyle.Left:
                            child.Bounds = new RectangleF(dockArea.Left, dockArea.Top, child.Width, dockArea.Height);
                            dockArea = RectangleF.FromLTRB(child.Right, dockArea.Top, dockArea.Right, dockArea.Height);
                            break;

                        case DockStyle.Top:
                            child.Bounds = new RectangleF(dockArea.Left, dockArea.Top, dockArea.Width, child.Height);
                            dockArea = RectangleF.FromLTRB(dockArea.Left, child.Bottom, dockArea.Width, dockArea.Bottom);
                            break;

                        case DockStyle.Right:
                            child.Bounds = new RectangleF(dockArea.Right - child.Width, dockArea.Top, child.Width, dockArea.Height);
                            dockArea = RectangleF.FromLTRB(dockArea.Left, dockArea.Top, child.Left, dockArea.Height);
                            break;

                        case DockStyle.Bottom:
                            child.Bounds = new RectangleF(dockArea.Left, dockArea.Bottom - child.Height, dockArea.Width, child.Height);
                            dockArea = RectangleF.FromLTRB(dockArea.Left, dockArea.Top, dockArea.Right, child.Top);
                            break;
                    }
                }
                else if (child.Anchor != AnchorStyles.None)
                {
                    if (e.BoundsChanged)
                    {
                        // Right only
                        if ((child.Anchor & AnchorStyles.Left) != AnchorStyles.Left && (child.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
                        {
                            float difference = e.NewBounds.Right - e.OldBounds.Right;
                            child.Bounds = RectangleF.FromLTRB(child.Left + difference, child.Top, child.Right + difference, child.Bottom);
                        }
                        // Left + Right
                        else if ((child.Anchor & AnchorStyles.Left) == AnchorStyles.Left && (child.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
                        {
                            float difference = e.NewBounds.Right - e.OldBounds.Right;
                            child.Bounds = RectangleF.FromLTRB(child.Left, child.Top, child.Right + difference, child.Bottom);
                        }

                        // Bottom only
                        if ((child.Anchor & AnchorStyles.Top) != AnchorStyles.Top && (child.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                        {
                            float difference = e.NewBounds.Bottom - e.OldBounds.Bottom;
                            child.Bounds = RectangleF.FromLTRB(child.Left, child.Top + difference, child.Right, child.Bottom + difference);
                        }
                        // Top + Bottom
                        else if ((child.Anchor & AnchorStyles.Top) == AnchorStyles.Top && (child.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                        {
                            float difference = e.NewBounds.Bottom - e.OldBounds.Bottom;
                            child.Bounds = RectangleF.FromLTRB(child.Left, child.Top, child.Right, child.Bottom + difference);
                        }
                    }
                }
                else
                {
                    child.Size = child.GetDesiredSize(new SizeF(float.PositiveInfinity, float.PositiveInfinity));
                }
            }

            OnLayout(e);

            if (Layout != null)
                Layout(this, e);
        }

        public void PerformRender(RenderEventArgs e)
        {
            e.Graphics.TranslateTransform(X, Y);
            e.Graphics.MultiplyTransform(Transform);

            if (ClipToBounds)
                e.Graphics.SetClip(ClientRectangle);

            GraphicsContainer container = e.Graphics.BeginContainer();

            if (IsVisible)
                OnRender(e);

            foreach (UIElement child in Children)
                child.PerformRender(e);

            e.Graphics.EndContainer(container);
            e.Graphics.ResetTransform();
            e.Graphics.ResetClip();
        }

        protected Rectangle GetClipRectangle(RectangleF rect)
        {
            UIElement element = this;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(rect);

                do
                {
                    using (Matrix matrix = element.Transform.Clone())
                    {
                        path.Transform(matrix);

                        using (Matrix translate = new Matrix(1.0f, 0.0f, 0.0f, 1.0f, element.X, element.Y))
                            path.Transform(translate);
                    }

                    element = element.Parent;
                } while (element != null);

                Rectangle ret = Utility.RoundRectangle(path.GetBounds());
                ret.Inflate(1, 1);
                return ret;
            }
        }

        public void Invalidate()
        {
            Invalidate(ClientRectangle);
        }

        public void Invalidate(RectangleF rect)
        {
            if (Container != null)
            {
                Container.Invalidate(GetClipRectangle(rect));
            }
        }

        public void InvalidateMeasure()
        {

        }

        internal void MouseDownInternal(MouseRoutedEventArgs e)
        {
            OnMouseDown(e);
        }

        internal void MouseUpInternal(MouseRoutedEventArgs e)
        {
            OnMouseUp(e);
        }

        internal void MouseMoveInternal(MouseRoutedEventArgs e)
        {
            OnMouseMove(e);
        }

        protected virtual void OnLayout(LayoutChangedEventArgs e)
        {
        }

        protected virtual void OnRender(RenderEventArgs e)
        {
            if (Background != null)
            {
                Utility.FillRectangle(e.Graphics, Background, ClientRectangle);
            }

            if (Render != null)
                Render(this, e);
        }

        protected virtual void OnMouseDown(MouseRoutedEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }

        protected virtual void OnMouseUp(MouseRoutedEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this, e);
        }

        protected virtual void OnMouseMove(MouseRoutedEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(this, e);
        }

        protected virtual void OnMouseEnter(EventArgs e)
        {
            if (MouseEnter != null)
                MouseEnter(this, e);
        }

        protected virtual void OnMouseLeave(EventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(this, e);
        }

        protected virtual void OnEnter(EventArgs e)
        {
            if (Enter != null)
                Enter(this, e);
        }

        protected virtual void OnLeave(EventArgs e)
        {
            if (Leave != null)
                Leave(this, e);
        }

        protected virtual void OnBoundsChanged(EventArgs e)
        {
            if (BoundsChanged != null)
                BoundsChanged(this, e);
        }

        protected virtual void OnParentChanged(EventArgs e)
        {
            if (ParentChanged != null)
                ParentChanged(this, e);
        }

        protected virtual void OnChildrenChanged(CollectionChangedEventArgs e)
        {
            EventHandler visibleChanged = (s, args) => PerformLayout(new LayoutChangedEventArgs());

            foreach (UIElement child in e.ItemsRemoved)
            {
                child.IsVisibleChanged -= visibleChanged;
            }

            foreach (UIElement child in e.ItemsAdded)
            {
                child.IsVisibleChanged += visibleChanged;
            }

            PerformLayout();

            if (ChildrenChanged != null)
                ChildrenChanged(this, e);
        }

        protected virtual void OnIsVisibleChanged(EventArgs e)
        {
            if (IsVisibleChanged != null)
                IsVisibleChanged(this, e);
        }

        protected virtual void OnBackgroundChanged(EventArgs e)
        {
            Invalidate();

            if (BackgroundChanged != null)
                BackgroundChanged(this, e);
        }

        protected virtual void OnLoaded(EventArgs e)
        {
            if (Loaded != null)
                Loaded(this, e);
        }

        public override string ToString()
        {
            return !String.IsNullOrEmpty(Name) ? Name : base.ToString();
        }
    }
}
