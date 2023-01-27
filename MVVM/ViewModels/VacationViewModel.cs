using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using System;
using System.Windows.Media;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class VacationViewModel
    {
        public string Name { get; set; }
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public Brush Color { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Status { get; set; }
        public string Creator_Id { get; set; }
        public int Count => GetCount();

        public VacationViewModel(string name, int user_Id_SAP, int vacation_Id, Brush color, DateTime dateStart, DateTime dateEnd, string status, string creator_Id)
        {
            Name = name;
            User_Id_SAP = user_Id_SAP;
            Vacation_Id = vacation_Id;
            Color = color;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Status = status;
            Creator_Id = creator_Id;
        }

        public override bool Equals(object obj)
        {
            return obj is VacationViewModel model &&
                   User_Id_SAP == model.User_Id_SAP &&
                   Name == model.Name &&
                   DateStart == model.DateStart &&
                   DateEnd == model.DateEnd;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(User_Id_SAP, Name, DateStart, DateEnd);
        }

        public Range<DateTime> ReturnVacationRange()
        {
            Range<DateTime> range = DateEnd > DateStart ? DateStart.To(DateEnd) : DateEnd.To(DateStart);
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
