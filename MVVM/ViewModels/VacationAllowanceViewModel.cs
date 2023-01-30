using System;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class VacationAllowanceViewModel : ViewModelBase, ICloneable
    {
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public string Vacation_Name { get; set; }

        private int _vacation_Year;
        public int Vacation_Year
        {
            get => _vacation_Year;
            set
            {
                _vacation_Year = value;
                OnPropertyChanged(nameof(Vacation_Year));
            }
        }

        private int _vacation_Days_Quantity;
        public int Vacation_Days_Quantity
        {
            get => _vacation_Days_Quantity;
            set
            {
                _vacation_Days_Quantity = value;
                OnPropertyChanged(nameof(Vacation_Days_Quantity));
            }
        }
        public Brush Vacation_Color { get; set; }
        public VacationAllowanceViewModel(int user_Id_SAP, string vacation_Name, int vacation_Id, int vacation_Year, int vacation_Days_Quantity, Brush vacation_color)
        {
            User_Id_SAP = user_Id_SAP;
            Vacation_Id = vacation_Id;
            Vacation_Name = vacation_Name;
            Vacation_Year = vacation_Year;
            Vacation_Days_Quantity = vacation_Days_Quantity;
            Vacation_Color = vacation_color;
        }

        public override bool Equals(object obj)
        {
            return obj is VacationAllowanceViewModel model &&
                   User_Id_SAP == model.User_Id_SAP &&
                   Vacation_Id == model.Vacation_Id &&
                   Vacation_Year == model.Vacation_Year;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(User_Id_SAP, Vacation_Id, Vacation_Year);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
