namespace Vacation_Portal.DTOs
{
    public class PersonDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Account { get; set; }
        public int Department_Id { get; set; }
        public bool Is_Supervisor { get; set; }
        public bool Is_HR { get; set; }
    }
}
