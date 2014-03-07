using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ElementFramework
{
    public class ThumbElement : UIElement
    {
        public event EventHandler<DragDeltaEventArgs> DragDelta;
        public event EventHandler<DragStartedEventArgs> DragStarted;
        public event EventHandler<DragCompletedEventArgs> DragCompleted;

        private bool isDragging;
        private PointF startOffset;
        private Point startPoint;
        private Point lastPoint;

        public ThumbElement()
        {
        }

        protected override void OnMouseDown(MouseRoutedEventArgs e)
        {
            base.OnMouseDown(e);

            if (CaptureMouse())
            {
                isDragging = true;

                startOffset = e.GetPosition(this);
                startPoint = e.ContainerLocation;
                lastPoint = startPoint;

                if (DragStarted != null)
                    DragStarted(this, new DragStartedEventArgs(startOffset.X, startOffset.Y));

                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseRoutedEventArgs e)
        {
            base.OnMouseUp(e);

            if (isDragging)
            {
                ReleaseMouseCapture();

                isDragging = false;

                PointF endPoint = e.ContainerLocation;

                if (DragCompleted != null)
                    DragCompleted(this, new DragCompletedEventArgs(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, false));

                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseRoutedEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                Point currentPoint = e.ContainerLocation;

                if (DragDelta != null)
                    DragDelta(this, new DragDeltaEventArgs(currentPoint.X - lastPoint.X, currentPoint.Y - lastPoint.Y));

                lastPoint = currentPoint;

                e.Handled = true;
            }
        }
    }

    public class DragDeltaEventArgs : EventArgs
    {
        public float HorizontalChange { get; private set; }
        public float VerticalChange { get; private set; }

        public DragDeltaEventArgs(float horizontalChange, float verticalChange)
        {
            HorizontalChange = horizontalChange;
            VerticalChange = verticalChange;
        }
    }

    public class DragCompletedEventArgs : EventArgs
    {
        public float HorizontalChange { get; private set; }
        public float VerticalChange { get; private set; }
        public bool Canceled { get; private set; }

        public DragCompletedEventArgs(float horizontalChange, float verticalChange, bool canceled)
        {
            HorizontalChange = horizontalChange;
            VerticalChange = verticalChange;
            Canceled = canceled;
        }
    }

    public class DragStartedEventArgs : EventArgs
    {
        public float HorizontalOffset { get; private set; }
        public float VerticalOffset { get; private set; }

        public DragStartedEventArgs(float horizontalOffset, float verticalOffset)
        {
            HorizontalOffset = horizontalOffset;
            VerticalOffset = verticalOffset;
        }
    }
}
