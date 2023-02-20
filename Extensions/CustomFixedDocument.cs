using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Vacation_Portal.Extensions
{
    [ContentProperty("Pages")]
    public class CustomFixedDocument : FixedDocument
    {
        private ObservableCollection<PageContent> _pages;

        public CustomFixedDocument()
        {
            this.Pages = new ObservableCollection<PageContent>();
        }

        public FixedDocument FixedDocument
        {
            get
            {
                var document = new FixedDocument();
                foreach (var p in Pages)
                {
                    var copy = XamlReader.Parse(XamlWriter.Save(p)) as PageContent;
                    document.Pages.Add(copy);
                }
                return document;
            }
        }

        public new ObservableCollection<PageContent> Pages
        {
            get => _pages;
            set
            {
                _pages = value;

                foreach (var page in _pages)
                {
                    base.Pages.Add(page);
                }

                _pages.CollectionChanged += (o, e) =>
                {
                    if (e.NewItems != null)
                    {
                        foreach (PageContent page in e.NewItems)
                        {
                            base.Pages.Add(page);
                        }
                    }
                };
            }
        }
    }
}
