using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public class Thickness
    {
        public float All
        {
            get
            {
                if ((Left == Top) && (Left == Right) && (Left == Bottom))
                    return Left;
                else
                    return -1;
            }
            set
            {
                Left = value;
                Top = value;
                Right = value;
                Bottom = value;
            }
        }

        private float _left;
        public float Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }

        private float _top;
        public float Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
            }
        }

        private float _right;
        public float Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }

        private float _bottom;
        public float Bottom
        {
            get
            {
                return _bottom;
            }
            set
            {
                _bottom = value;
            }
        }

        public static bool operator !=(Thickness p1, Thickness p2)
        {
            return !(p1 == p2);
        }

        public static bool operator ==(Thickness p1, Thickness p2)
        {
            return (p1.Left == p2.Left) && (p1.Top == p2.Top) && (p1.Right == p2.Right) && (p1.Bottom == p2.Bottom);
        }

        public Thickness()
        {
        }

        public Thickness(float all)
            : this(all, all, all, all)
        {
        }

        public Thickness(float left, float top, float right, float bottom)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is Thickness)
            {
                Thickness t = (Thickness)obj;
                return this == t;
            }

            return false;
        }

        public bool Equals(Thickness thickness)
        {
            return this == thickness;
        }

        public override int GetHashCode()
        {
            return this.Left.GetHashCode() ^ this.Top.GetHashCode() ^ this.Right.GetHashCode() ^ this.Bottom.GetHashCode();
        }
    }
}
