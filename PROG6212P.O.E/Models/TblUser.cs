using System;
using System.Collections.Generic;

namespace PROG6212P.O.E.Models
{
    //these getters and setters gat and set the data needed for the tables
    public partial class TblUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual Base Base { get; set; }
        public virtual Buy Buy { get; set; }
        public virtual Rent Rent { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
