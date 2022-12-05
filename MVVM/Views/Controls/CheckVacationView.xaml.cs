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
    /// Логика взаимодействия для CheckVacationView.xaml
    /// </summary>
    public partial class CheckVacationView : UserControl
    {
        public CheckVacationView()
        {
            InitializeComponent();
        }
        public void VisibilityButton14()
        {
            isNotCheck14.Visibility = Visibility.Hidden;
            isCheck14.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = false;
        }

        internal void NotVisibilityButton14()
        {
            isCheck14.Visibility = Visibility.Hidden;
            isNotCheck14.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = false;
        }

        public void VisibilityButton7()
        {
            isNotCheck7.Visibility = Visibility.Hidden;
            isCheck7.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = true;
        }

        internal void NotVisibilityButton7()
        {
            isCheck7.Visibility = Visibility.Hidden;
            isNotCheck7.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = true;
        }

        public void ClearVisibility()
        {
            SaveBtn.IsEnabled = false;
            isCheck14.Visibility = Visibility.Hidden;
            isNotCheck14.Visibility = Visibility.Hidden;
            isCheck7.Visibility = Visibility.Hidden;
            isNotCheck7.Visibility = Visibility.Hidden;
        }
    }
}
