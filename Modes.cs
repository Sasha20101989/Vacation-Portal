using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vacation_Portal
{
    public enum Modes
    {
        [Description("Страница персонального планирования отпуска")]
        Personal = 1,
        [Description("Страница руководителя")]
        Subordinate = 2
    }
}
