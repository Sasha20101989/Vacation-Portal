using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vacation_Portal.MVVM.Views.Controls {
    /// <summary>
    /// Логика взаимодействия для DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl {
        public DayControl() {
            InitializeComponent();
        }
        public void Day(DateTime date) {
            tbDay.Text = date.Day.ToString("d");
            tbDay.Tag = date.Month + "." + date.Year;
            tbDay.ToolTip = "Рабочий";
        }
        public void DayWork(DateTime date) {
            tbDay.Foreground = Brushes.Black;
            tbDay.Background = Brushes.Transparent;
            tbDay.ToolTip = "Рабочий в выходной";
        }
        public void DaysOff(DateTime date) {
            string dayOfWeek = date.DayOfWeek.ToString();
            if(dayOfWeek == "Saturday" || dayOfWeek == "Sunday") {
                tbDay.Foreground = Brushes.Red;
                tbDay.ToolTip = "Выходной";
            }
        }
        public void DayOff(DateTime date) {
            tbDay.Foreground = Brushes.Red;
            tbDay.ToolTip = "Выходной";
        }
        public void DayOffNotInPlan(DateTime date) {
            tbDay.Foreground = Brushes.Red;
            tbDay.ToolTip = "Внеплановый";
        }
        public void Holiday(DateTime date) {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            tbDay.Foreground = Brushes.Red;
            tbDay.Background = (Brush) converter.ConvertFromString("#FCA795");
            tbDay.Width = 19;
            tbDay.Height = 18;
            tbDay.ToolTip = "Праздник";
        }
    }
}
