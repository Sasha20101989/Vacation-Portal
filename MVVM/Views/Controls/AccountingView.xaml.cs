﻿using System.Windows;
using System.Windows.Controls;

namespace Vacation_Portal.MVVM.Views.Controls {
    /// <summary>
    /// Логика взаимодействия для AccountingView.xaml
    /// </summary>
    public partial class AccountingView : UserControl {
        public AccountingView() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if(ListView.Height is double.NaN) {
                //ListView.Height = ListView.ActualHeight * App.API.Person.Vacations.Count / 3.3;
                ListView.Height = 383;
            }
        }
    }
}
