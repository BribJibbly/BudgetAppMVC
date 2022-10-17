using PROG6212P.O.E.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG6212P.O.E.ViewModels
{
    public class Tables
    {
        public int id { get; set; }
        public Base Base { get; set; }
        public Rent Rent { get; set; }
        public Vehicle Vehicle { get; set; }
        public Buy Buy { get; set; }
       
    }
}
