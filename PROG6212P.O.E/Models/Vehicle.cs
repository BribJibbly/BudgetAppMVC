using System;
using System.Collections.Generic;

namespace PROG6212P.O.E.Models
{
    //these getters and setters gat and set the data needed for the tables
    public partial class Vehicle
    {
        public string UserName { get; set; }
        public string Model { get; set; }
        public decimal? PurPrice { get; set; }
        public decimal? Deposit { get; set; }
        public int? Interest { get; set; }
        public decimal? Insurance { get; set; }

        public virtual TblUser UserNameNavigation { get; set; }
    }
}
