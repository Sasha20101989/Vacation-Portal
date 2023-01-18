using System;

namespace Vacation_Portal.DTOs
{
    public class FullPersonDTO
    {
        //PersonTDO
        public int User_Id_SAP { get; set; }
        public string User_Id_Account { get; set; }
        public string User_Name { get; set; }
        public string User_Surname { get; set; }
        public string User_Patronymic_Name { get; set; }
        public int User_Department_Id { get; set; }
        public int User_Virtual_Department_Id { get; set; }
        public int User_Sub_Department_Id { get; set; }
        public string User_App_Color { get; set; }
        public int User_Supervisor_Id_SAP { get; set; }    
        public string Role_Name { get; set; }


        //VacationDTO
        public int Remained_User_Id_SAP { get; set; }
        public int Remained_Vacation_Id { get; set; }
        public int Remained_Vacation_Year { get; set; }
        public DateTime Vacation_Start_Date { get; set; }
        public DateTime Vacation_End_Date { get; set; }
        public string Vacation_Status_Id { get; set; }
        public string Vacation_Name { get; set; }
        public string Vacation_Color { get; set; }
        public int Remained_Vacation_Days_Quantity { get; set; }
        public string Creator_Id { get; set; }

        //VacationAllowanceDTO
        public int Remained_Allowance_User_Id_SAP { get; set; }
        public int Remained_Allowance_Vacation_Id { get; set; }
        public int Remained_Allowance_Vacation_Year { get; set; }
        public int Remained_Allowance_Vacation_Days_Quantity { get; set; }
    }
}
