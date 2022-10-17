﻿using PROG6212P.O.E.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG6212P.O.E.ViewModels
{
    public class BudgBuyVehViewModel
    {
        //these getters and setters get and set the data needed from other tables or will be used to get and set
        //values from the calculations done in the class library
        public int id { get; set; }
        public Base Base { get; set; }
        public Buy Buy { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal? Expense { get; set; }
        public double Taxes { get; set; }
        public double Remaining { get; set; }
        public double Property { get; set; }
        public double VehicleCost { get; set; }
    }
}
