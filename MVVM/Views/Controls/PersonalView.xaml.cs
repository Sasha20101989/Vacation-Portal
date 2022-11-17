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
            if (this.ListView.Height is double.NaN)
            {
                this.ListView.Height = this.ListView.ActualHeight * 7.5;
            }          
        }
    }
}
