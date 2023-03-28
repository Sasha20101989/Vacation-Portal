using System.Collections.Generic;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.DTOs {
    public class PersonDTO {
        public int Id { get; set; }
        public string Account_Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic_Name { get; set; }
        public int Department_Id { get; set; }
        public string Department_Name { get; set; }
        public int Sub_Department_Id { get; set; }
        public int Virtual_Department_Id { get; set; }
        public string Virtual_Department_Name { get; set; }
        public string Role_Name { get; set; }
        public int Supervisor_Id { get; set; }
        public string App_Color { get; set; }
        public string Position { get; set; }
        public virtual List<VacationViewModel> User_Vacations { get; set; }

    }
}
