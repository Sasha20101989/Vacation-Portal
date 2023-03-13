using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Vacation_Portal.MVVM.Views {
    /// <summary>
    /// Логика взаимодействия для PrintPreView.xaml
    /// </summary>
    public partial class PrintPreView : UserControl {
        public PrintPreView() {
            InitializeComponent();
        }

        private void Print_Click(object sender, RoutedEventArgs e) {
            // создаем экземпляр PrintDialog
            PrintDialog printDialog = new PrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            printDialog.UserPageRangeEnabled = true;
            printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

            printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
            printDialog.PrintTicket.PageScalingFactor = 100;
            printDialog.PrintTicket.PageBorderless = PageBorderless.None;
            printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);


            if(printDialog.ShowDialog() == true) {
                // вызываем метод PrintDocument
                printDialog.PrintDocument(((IDocumentPaginatorSource) this.MyDocument.Document).DocumentPaginator, "Заявление на выплату компенсации");
            }
        }
    }
}
