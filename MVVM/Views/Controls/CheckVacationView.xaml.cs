using System.Windows;
using System.Windows.Controls;

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
        public void VisibilityButtonFirstCheck()
        {
            isNotCheck14.Visibility = Visibility.Hidden;
            isCheck14.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = false;
        }

        internal void NotVisibilityButtonFirstCheck()
        {
            isCheck14.Visibility = Visibility.Hidden;
            isNotCheck14.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = false;
        }

        public void VisibilityButtonSecondCheck()
        {
            isNotCheck7.Visibility = Visibility.Hidden;
            isCheck7.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = true;
        }

        internal void NotVisibilityButtonSecondCheck()
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
