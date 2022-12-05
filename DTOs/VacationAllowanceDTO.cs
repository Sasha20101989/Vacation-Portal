﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Vacation_Portal.DTOs
{
    public class VacationAllowanceDTO
    {
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public string Vacation_Name { get; set; }
        public string Vacation_Year { get; set; }
        public int Vacation_Days_Quantity { get; set; }
        public string Vacation_Color { get; set; }
    }
}