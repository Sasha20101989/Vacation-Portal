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
        public void day(DateTime date)
        {
            tbDay.Text = date.Day.ToString("d");
            tbDay.Tag = date.Month;
            tbDay.ToolTip = "Рабочий";
        }
        public void dayWork(DateTime date)
        {
            tbDay.Foreground = Brushes.Black;
            tbDay.Background = Brushes.Transparent;
            tbDay.ToolTip = "Рабочий в выходной";
        }
        public void daysOff(DateTime date)
        {
            string dayOfWeek = date.DayOfWeek.ToString();
            if (dayOfWeek == "Saturday" || dayOfWeek == "Sunday")
            {
                tbDay.Foreground = Brushes.Red;
                tbDay.ToolTip = "Выходной";
            }
        }
        public void dayOff(DateTime date)
        {
            tbDay.Foreground = Brushes.Red;
            tbDay.ToolTip = "Выходной";
        }
        public void dayOffNotInPlan(DateTime date)
        {
            tbDay.Foreground = Brushes.Red;
            tbDay.ToolTip = "Внеплановый";
        }
        public void holiday(DateTime date)
        {
            var converter = new System.Windows.Media.BrushConverter();
            tbDay.Foreground = Brushes.Red;
            tbDay.Background = (Brush)converter.ConvertFromString("#FCA795");
            tbDay.Width = 19;
            tbDay.Height = 18;
            tbDay.ToolTip = "Праздник";
        }
    }
}
