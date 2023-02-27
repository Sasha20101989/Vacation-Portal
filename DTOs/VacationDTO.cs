using System;
using System.Collections.Generic;

namespace Vacation_Portal.DTOs
{
    public class VacationDTO
    {
        public int Id { get; set; }
        public string Vacation_Name { get; set; }
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public int Vacation_Year { get; set; }
        public string Vacation_Color { get; set; }
        public DateTime Vacation_Start_Date { get; set; }
        public DateTime Vacation_End_Date { get; set; }
        public string Vacation_Status_Name { get; set; }
        public string Creator_Id { get; set; }
        public int Count => GetCount();

        public IEnumerable<DateTime> GetDateRange(DateTime start, DateTime end)
        {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                yield return date;
            }
        }

        private int GetCount()
        {
            int count = 0;
            IEnumerable<DateTime> dateRange = GetDateRange(Vacation_Start_Date, Vacation_End_Date);
            foreach(DateTime date in dateRange)
            {
                count++;
            }
            return count;
        }
    }
}
