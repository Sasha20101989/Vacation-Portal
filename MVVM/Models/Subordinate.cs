using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.MVVM.Models
{
    public class Subordinate
    {
        public int Id_SAP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public string FullName => ToString();
        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Subordinate(int idSAP,string name, string surname, string patronymic, string position)
        {
            Id_SAP = idSAP;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Position = position;
        }
    }
}
