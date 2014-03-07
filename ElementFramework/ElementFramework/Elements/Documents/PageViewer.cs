using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public class PageViewer : UIElement
    {
        private ObservableList<PageElement> _pages = new ObservableList<PageElement>();
        public ObservableList<PageElement> Pages
        {
            get
            {
                return _pages;
            }
        }

        public PageViewer()
        {
            Pages.CollectionChanged += new EventHandler<CollectionChangedEventArgs>(Pages_CollectionChanged);
        }

        protected override void OnLayout(LayoutChangedEventArgs e)
        {
            base.OnLayout(e);

            foreach (PageElement page in Pages)
            {
                // Layout each page
            }
        }

        private void Pages_CollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            foreach (PageElement page in e.ItemsRemoved)
            {
                Children.Remove(page);
            }

            foreach (PageElement page in e.ItemsAdded)
            {
                Children.Add(page);
            }

            PerformLayout(new LayoutChangedEventArgs());
            Invalidate();
        }
    }
}
