using System;

namespace Vacation_Portal.DTOs
{
    public class VacationDTO
    {
        public string Vacation_Name { get; set; }
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public int Vacation_Year { get; set; }
        public string Vacation_Color { get; set; }
        public DateTime Vacation_Start_Date { get; set; }
        public DateTime Vacation_End_Date { get; set; }
        public string Vacation_Status { get; set; }
        public string Creator_Id { get; set; }
    }
}
