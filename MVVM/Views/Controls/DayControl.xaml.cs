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
    /// Логика взаимодействия для DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public DayControl()
        {
            InitializeComponent();
        }
        public void days(DateTime date)
        {
            tbDay.Text = date.Day.ToString("d");
            tbDay.Tag = date.Month;
        }
    }
}
