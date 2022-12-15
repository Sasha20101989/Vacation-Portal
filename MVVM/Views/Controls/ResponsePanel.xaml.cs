using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;

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
        }
        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button) sender).IsEnabled = false;
            await SetAcceptedStateAsync();
            SubmitResponse(InviteResponseKind.Accepted);
            ((Button) sender).IsEnabled = true;
        }

        private async Task SetAcceptedStateAsync()
        {
            VisualStateManager.GoToState(this, "Accepted", true);
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(30);
                DeclineBorder.Opacity -= 0.1;
                DeclineRoot.Opacity -= 0.1;
            }
        }
        private async Task SetDeclinedStateAsync()
        {
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(30);
                AcceptBorder.Opacity -= 0.1;
                AcceptRoot.Opacity -= 0.1;
            }
            ScaleTransform scaleTransform1 = new ScaleTransform(1.5, 2.0, 50, 50);
            AcceptRoot.RenderTransform = scaleTransform1;
        }

        private async void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button) sender).IsEnabled = false;
            await SetDeclinedStateAsync();
            SubmitResponse(InviteResponseKind.Declined);
            ((Button) sender).IsEnabled = true;
        }
        private void SubmitResponse(InviteResponseKind response)
        {
            Brush brushAcceptedColor = Brushes.Green;
            Brush brushDeclinedColor = Brushes.Red;
            if(response == InviteResponseKind.Accepted)
            {
                AcceptText.Text = "Accepted";
                AcceptBorder.Background = brushAcceptedColor;

            } else if(response == InviteResponseKind.Declined)
            {
                DeclineText.Text = "Declined";
                DeclineBorder.Background = brushDeclinedColor;
            }
        }
    }
}
