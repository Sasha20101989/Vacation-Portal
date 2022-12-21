using System.Collections.Generic;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.DTOs
{
    public class PersonDTO
    {
        public int User_Id_SAP { get; set; }
        public string User_Id_Account { get; set; }
        public string User_Name { get; set; }
        public string User_Surname { get; set; }
        public string User_Patronymic_Name { get; set; }
        public int User_Department_Id { get; set; }
        public int User_Sub_Department_Id { get; set; }
        public int User_Virtual_Department_Id { get; set; }
        public string User_Position_Id { get; set; }
        public string Role_Name { get; set; }
        public int User_Supervisor_Id_SAP { get; set; }
        public string User_App_Color { get; set; }
        public virtual List<VacationViewModel> User_Vacations { get; set; }

    }
}
