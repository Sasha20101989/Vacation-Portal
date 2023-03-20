using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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

namespace Vacation_Portal.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для HorizontalCalendarView.xaml
    /// </summary>
    public partial class HorizontalCalendarView : UserControl
    {
        public HorizontalCalendarView()
        {
            InitializeComponent();
        }
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var firstVisibleHeader = GetFirstVisibleItemHeader(myItemsControl);
            if(firstVisibleHeader != null)
            {

            }
        }


        private FrameworkElement GetFirstVisibleItemHeader(ItemsControl itemsControl)
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
            int firstVisibleIndex = (int) scrollViewer.HorizontalOffset;
            #endregion

            var container = (FrameworkElement) itemsControl.ItemContainerGenerator.ContainerFromIndex(10);

            return container;

        }

    }
}
