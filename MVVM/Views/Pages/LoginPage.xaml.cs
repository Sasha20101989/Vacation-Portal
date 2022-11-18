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
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.MVVM.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        public AuthenticationViewModel ViewModel => App.AuthenticationViewModel;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
