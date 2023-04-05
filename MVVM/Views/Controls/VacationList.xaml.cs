using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vacation_Portal.MVVM.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для FlipContent.xaml
    /// </summary>
    public partial class VacationList : UserControl
    {
        public VacationList()
        {
            InitializeComponent();
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            ListView.IsHitTestVisible = true;
        }

        private void PopupBox_Closed(object sender, RoutedEventArgs e)
        {
            ListView.IsHitTestVisible = true;

        }
    }
}
