namespace Vacation_Portal.MVVM.Models {
    public class Settings {
        public string Account { get; set; }
        public string Color { get; set; }
        public Settings(string account, string color) {
            Account = account;
            Color = color;
        }
    }
}
