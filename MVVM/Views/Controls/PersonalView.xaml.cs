using System.Windows;
using System.Windows.Controls;

namespace Vacation_Portal.MVVM.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для PersonalView.xaml
    /// </summary>
    public partial class PersonalView : UserControl
    {
        public PersonalView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(ListView.Height is double.NaN)
            {
                //ListView.Height = ListView.ActualHeight * App.API.Person.Vacations.Count / 3.3;
                ListView.Height = 433;
            }
        }
    }
}
