using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budg_Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROG6212P.O.E.Models;
using PROG6212P.O.E.ViewModels;

namespace PROG6212P.O.E.Controllers
{
    public class BudgRentController : Controller
    {
        //database context
        STORAGETASK3Context db = new STORAGETASK3Context();
        public BudgRentController(STORAGETASK3Context context)
        {
            db = context;
        }

                //this is the code that allows the view model to get values from two different tables and order them based on user
        public IActionResult Display()
        {
            //these lists allow for testing if values are returned (List<Rent>), used in the foreach to get data(List<Base>) or
            //are used to return all results(List<BudgRentViewModel>)
            List<BudgRentViewModel> BRData = new List<BudgRentViewModel>();
            List<Expenses> expenses = new List<Expenses>();
            List<Base> bases = db.Base.ToList();
            List<Rent> rents = db.Rent.ToList();
            string User = HttpContext.Session.GetString("LoggedInUser");
            //This if statement will make it so that the page cannot be accessed unless a user is logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                //foreach makes code inside apply to every item within list<Base>
                foreach (Base item in bases)
                {
                    BudgRentViewModel BudgetRent = new BudgRentViewModel();
                    BudgetRent.Base = item;

                    //this code will match what ever is in table Rent with what ever is in table Base based on the username in both
                    Rent rent = db.Rent.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    BudgetRent.Rent = rent;
                    BRData.Add(BudgetRent);

                    //this code sends data to the class library calc and returns results
                    Expenses expense = new Expenses(item.Groceries, item.WaterLights, item.Travel, item.Phone, item.Other);
                    BudgetRent.Expense = (decimal)expense.Expenses_Total();
                    Taxes tax = new Taxes((int)item.Income);
                    BudgetRent.Taxes = Math.Round((double)tax.Tax_Total(), 2);

                    //this returns calculation of total amount of money remaining
                    BudgetRent.Remaining = ((double)item.Income - (double)BudgetRent.Expense - BudgetRent.Taxes - (double)rent.ReantAmnt);
                }
                return View(BRData.Where(c => c.Base.UserName.Equals(User)));
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        
    }
}
