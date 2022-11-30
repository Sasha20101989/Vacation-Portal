using System.Windows;

namespace Vacation_Portal.MVVM.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItemsSearchBox.Focus();
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            NavDrawer.IsLeftDrawerOpen = false;
            //NavRail.Visibility = Visibility.Collapsed;
        }

        private void CLoseNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            NotifycationPanel.Visibility = Visibility.Collapsed;
        }
    }
}
