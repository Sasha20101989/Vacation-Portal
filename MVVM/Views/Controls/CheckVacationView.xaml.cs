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
        public void VisibilityButtonFirstCheck(bool isSupervisor)
        {
            ClearVisibility();
            isCheck14.Visibility = Visibility.Visible;
            if(isSupervisor)
            {
                ButtonSubmit.Text = "Согласовать";
            }
            SaveBtn.IsEnabled = false;
        }

        internal void NotVisibilityButtonFirstCheck(bool isSupervisor)
        {
            ClearVisibility();
            isNotCheck14.Visibility = Visibility.Visible;
            if(isSupervisor)
            {
                ButtonSubmit.Text = "Согласовать";
            }
            SaveBtn.IsEnabled = false;
        }

        public void VisibilityButtonSecondCheck(bool isSupervisor)
        {
            ClearVisibility();
            isCheck14.Visibility = Visibility.Visible;
            isCheck7.Visibility = Visibility.Visible;
            if(isSupervisor)
            {
                ButtonSubmit.Text = "Согласовать";
            }
            SaveBtn.IsEnabled = true;
        }

        internal void NotVisibilityButtonSecondCheck(bool isSupervisor)
        {
            ClearVisibility();
            isCheck14.Visibility = Visibility.Visible;
            isNotCheck7.Visibility = Visibility.Visible;
            if(isSupervisor)
            {
                ButtonSubmit.Text = "Согласовать";
            }
            SaveBtn.IsEnabled = true;
        }

        public void VisibilityExclamationButtonSecondCheck(bool isSupervisor)
        {
            isCheck14.Visibility = Visibility.Visible;
            isExclamation7.Visibility = Visibility.Visible;
            isExclamation7.ToolTip = "Не запланированно 7 дней непрерывного основного отпуска, но вы можете согласовать текущее состояние.";
            if(isSupervisor)
            {
                ButtonSubmit.Text = "Согласовать";
            }
            SaveBtn.IsEnabled = true;
        }

        public void ClearVisibility()
        {
            SaveBtn.IsEnabled = false;
            isCheck14.Visibility = Visibility.Hidden;
            isNotCheck14.Visibility = Visibility.Hidden;
            isCheck7.Visibility = Visibility.Hidden;
            isNotCheck7.Visibility = Visibility.Hidden;
            isExclamation7.Visibility = Visibility.Hidden;
        }
    }
}
