using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Vacation_Portal.Extensions {
    [ContentProperty("Pages")]
    public class CustomFixedDocument : FixedDocument {
        private ObservableCollection<PageContent> _pages;

        public CustomFixedDocument() {
            Pages = new ObservableCollection<PageContent>();
        }

        public FixedDocument FixedDocument {
            get {
                FixedDocument document = new FixedDocument();
                foreach(PageContent p in Pages) {
                    PageContent copy = XamlReader.Parse(XamlWriter.Save(p)) as PageContent;
                    document.Pages.Add(copy);
                }
                return document;
            }
        }

        public new ObservableCollection<PageContent> Pages {
            get => _pages;
            set {
                _pages = value;

                foreach(PageContent page in _pages) {
                    base.Pages.Add(page);
                }

                _pages.CollectionChanged += (o, e) => {
                    if(e.NewItems != null) {
                        foreach(PageContent page in e.NewItems) {
                            base.Pages.Add(page);
                        }
                    }
                };
            }
        }
    }
}
