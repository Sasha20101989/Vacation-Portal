using System.Windows.Media;

namespace Vacation_Portal.MVVM.Models {
    public class VacationAllowance {
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public string Vacation_Name { get; set; }
        public int Vacation_Year { get; set; }
        public int Vacation_Days_Quantity { get; set; }
        public Brush Vacation_Color { get; set; }
        public VacationAllowance(int user_Id_SAP, string vacation_Name, int vacation_Id, int vacation_Year, int vacation_Days_Quantity, Brush vacation_color) {
            User_Id_SAP = user_Id_SAP;
            Vacation_Id = vacation_Id;
            Vacation_Name = vacation_Name;
            Vacation_Year = vacation_Year;
            Vacation_Days_Quantity = vacation_Days_Quantity;
            Vacation_Color = vacation_color;
        }
    }
}
