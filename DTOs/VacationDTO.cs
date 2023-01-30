using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
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
        public string Vacation_Status_Name { get; set; }
        public string Creator_Id { get; set; }
        public int Count => GetCount();

        public Range<DateTime> ReturnVacationRange()
        {
            Range<DateTime> range = Vacation_End_Date > Vacation_Start_Date ? Vacation_Start_Date.To(Vacation_End_Date) : Vacation_End_Date.To(Vacation_Start_Date);
            return range;
        }

        private int GetCount()
        {
            int count = 0;
            Range<DateTime> range = ReturnVacationRange();
            foreach(DateTime date in range.Step(x => x.AddDays(1)))
            {
                count++;
            }
            return count;
        }
    }
}
