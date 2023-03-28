namespace Vacation_Portal.DTOs {
    public class VacationAllowanceDTO {
        public int User_Id { get; set; }
        public int Vacation_Type_Id { get; set; }
        public string Name { get; set; }
        public int Vacation_Year { get; set; }
        public int Vacation_Days_Quantity { get; set; }
        public string Color { get; set; }
    }
}
