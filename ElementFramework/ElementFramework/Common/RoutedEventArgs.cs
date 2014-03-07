using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public class RoutedEventArgs : EventArgs
    {
        private object _originalSource;
        public object OriginalSource
        {
            get
            {
                return _originalSource;
            }
            private set
            {
                _originalSource = value;
            }
        }

        private object _source;
        public object Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        private bool _handled;
        public bool Handled
        {
            get
            {
                return _handled;
            }
            set
            {
                _handled = value;
            }
        }

        public RoutedEventArgs()
        {
        }

        public RoutedEventArgs(object source)
        {
            OriginalSource = source;
            Source = source;
        }
    }
}
