using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;
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
