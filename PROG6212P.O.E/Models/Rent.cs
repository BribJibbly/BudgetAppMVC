using System;
using System.Collections.Generic;

namespace PROG6212P.O.E.Models
{
    //these getters and setters gat and set the data needed for the tables
    public partial class Rent
    {
        public string UserName { get; set; }
        public decimal? ReantAmnt { get; set; }

        public virtual TblUser UserNameNavigation { get; set; }
    }
}
