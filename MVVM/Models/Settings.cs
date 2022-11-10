namespace Vacation_Portal.MVVM.Models
{
    public class Settings
    {
        public string Account { get; set; }
        public double FontSize { get; set; }
        public string Color { get; set; }
        public Settings(string account, double fontSize, string color)
        {
            Account = account;
            FontSize = fontSize;
            Color = color;
        }
    }
}
