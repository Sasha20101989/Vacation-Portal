using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vacation_Portal.MVVM.Models.HorizontalCalendar;

namespace Vacation_Portal.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для HorizontalCalendarView.xaml
    /// </summary>
    public partial class HorizontalCalendarView : UserControl
    {
        private DispatcherTimer _timer;
        public HorizontalCalendarView()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(300);
            _timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            var monthName = GetFirstVisibleMonth(myItemsControl);
            CurrentMonth.Text = monthName;
        }
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }
        private string GetFirstVisibleMonth(ItemsControl itemsControl)
        {
            var scroll = itemsControl.Parent;
            var scroll1 = scroll.GetParentObject();
            var scroll2 = scroll1.GetParentObject();
            var scroll3 = scroll2.GetParentObject();
            var scroll4 = scroll3.GetParentObject();
            var scroll5 = scroll4.GetParentObject();
            var scroll6 = scroll5.GetParentObject();

            var scrollViewer = scroll6.GetParentObject() as ScrollViewer;
            Decorator border = VisualTreeHelper.GetChild(itemsControl, 0) as Decorator;
            var presenter = VisualTreeHelper.GetChild(border, 0);
            var virtualizingStackPanel = VisualTreeHelper.GetChild(presenter, 0) as VirtualizingStackPanel;

            if(virtualizingStackPanel == null)
                return null;


            #region получение первого видимого
            if(scrollViewer == null)
            {
                return null;
            }
            double offset = scrollViewer.HorizontalOffset;
            double viewportWidth = scrollViewer.ViewportWidth;
            var container = (FrameworkElement) itemsControl.ItemContainerGenerator.ContainerFromIndex(0);
            if(container == null)
                return null;
            double itemWidth = container.ActualWidth;

            int firstVisibleIndex = (int) (offset / itemWidth);
            int itemsPerViewPort = (int) (viewportWidth / itemWidth);
            int lastVisibleIndex = firstVisibleIndex + itemsPerViewPort;
            #endregion
            string monthName = "";
            var firstVisibleContainer = (FrameworkElement) itemsControl.ItemContainerGenerator.ContainerFromIndex(firstVisibleIndex + 3);
            var lastVisibleContainer = (FrameworkElement) itemsControl.ItemContainerGenerator.ContainerFromIndex(lastVisibleIndex);
            if(firstVisibleContainer != null)
            {
                var context = firstVisibleContainer.DataContext as HorizontalDay;
                DateTime date = context.Date;
                CultureInfo culture = new CultureInfo("ru-Ru");
                monthName = culture.DateTimeFormat.GetMonthName(date.Month);
            }

            return monthName;
        }

        private void DataGridRow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is DataGridRow row)
            {
                if(row.IsSelected)
                {
                    row.IsSelected = false;
                }
            }
        }
    }
}
