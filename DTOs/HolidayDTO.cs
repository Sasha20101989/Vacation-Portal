using System;

namespace Vacation_Portal.DTOs
{
    public class HolidayDTO
    {
        public int Id { get; set; }
        public string Holiday_Name { get; set; }
        public DateTime Holiday_Date { get; set; }
        public int Holiday_Year { get; set; }
    }
}
