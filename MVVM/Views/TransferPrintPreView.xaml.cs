using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vacation_Portal.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для TransferPrintPreView.xaml
    /// </summary>
    public partial class TransferPrintPreView : UserControl {
        public TransferPrintPreView()
        {
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
                printDialog.PrintDocument(((IDocumentPaginatorSource) this.MyDocument.Document).DocumentPaginator, "Заявление на перенос");

            }
        }
    }
}
