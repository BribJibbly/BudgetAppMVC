using System;
using System.Collections.Generic;

namespace PROG6212P.O.E.Models
{
    //these getters and setters gat and set the data needed for the tables
    public partial class Buy
    {
        public string UserName { get; set; }
        public decimal? PropPrice { get; set; }
        public decimal? Deposit { get; set; }
        public int? Interest { get; set; }
        public int? RepayMonths { get; set; }

        public virtual TblUser UserNameNavigation { get; set; }
    }
}
