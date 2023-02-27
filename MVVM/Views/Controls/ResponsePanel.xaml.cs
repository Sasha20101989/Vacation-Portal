using System.Windows.Controls;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.MVVM.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для ResponsePanel.xaml
    /// </summary>
    public partial class ResponsePanel : UserControl
    {
        public ResponsePanel()
        {
            InitializeComponent();
            DataContext = new ResponsePanelViewModel();
        }
    }
}
