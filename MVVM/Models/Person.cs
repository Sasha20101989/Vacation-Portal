namespace Vacation_Portal.MVVM.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Account { get; set; }
        public int Department_Id { get; set; }
        public bool Is_Supervisor { get; set; }
        public bool Is_HR { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Person(string name, string surname, string patronymic, string account,int departmentId, bool isSupervisor, bool isHR)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Account = account;
            Department_Id = departmentId;
            Is_Supervisor = isSupervisor;
            Is_HR = isHR;
        }
    }
}
