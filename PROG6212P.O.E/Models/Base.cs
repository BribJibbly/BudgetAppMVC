using System;
using System.Collections.Generic;


namespace PROG6212P.O.E.Models
{
    //these getters and setters gat and set the data needed for the tables
    public partial class Base
    {
        public string UserName { get; set; }
        public decimal? Income { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Groceries { get; set; }
        public decimal? WaterLights { get; set; }
        public decimal? Travel { get; set; }
        public decimal? Phone { get; set; }
        public decimal? Other { get; set; }

        public virtual TblUser UserNameNavigation { get; set; }
    }
}
