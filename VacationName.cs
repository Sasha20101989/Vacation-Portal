using System.ComponentModel;

namespace Vacation_Portal {
    public enum VacationName {
        [Description("Основной")]
        Principal = 2,
        [Description("Вредность")]
        Harmfulness = 3,
        [Description("Ненормированность")]
        Irregularity = 4,
        [Description("Стаж")]
        Experience = 5
    }
}
