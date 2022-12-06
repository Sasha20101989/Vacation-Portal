using System;
using System.Collections.Generic;
using System.Text;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Exceptions
{
    public class VacationConflictException : Exception
    {
        public Vacation ExistingVacation { get; }
        public Vacation IncomingVacation { get; }

        public VacationConflictException(Vacation existingVacation, Vacation incomingVacation)
        {
            ExistingVacation = existingVacation;
            IncomingVacation = incomingVacation;
        }

        public VacationConflictException(string message, Vacation existingVacation, Vacation incomingVacation) : base(message)
        {
            ExistingVacation = existingVacation;
            IncomingVacation = incomingVacation;
        }

        public VacationConflictException(string message, Exception innerException, Vacation existingVacation, Vacation incomingVacation) : base(message, innerException)
        {
            ExistingVacation = existingVacation;
            IncomingVacation = incomingVacation;
        }
    }
}
