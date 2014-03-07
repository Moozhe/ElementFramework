using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ElementFramework
{
    public class RenderEventArgs : EventArgs
    {
        private Graphics _graphics;
        public Graphics Graphics
        {
            get
            {
                return _graphics;
            }
            private set
            {
                _graphics = value;
            }
        }

        public RenderEventArgs(Graphics graphics)
        {
            Graphics = graphics;
        }
    }
}
