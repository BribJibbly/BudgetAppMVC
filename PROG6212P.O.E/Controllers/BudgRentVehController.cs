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
    public class BudgRentVehController : Controller
    {
        //database context
        STORAGETASK3Context db = new STORAGETASK3Context();
        public BudgRentVehController(STORAGETASK3Context context)
        {
            db = context;
        }

        //this is the code that allows the view model to get values from two different tables and order them based on user
        public IActionResult Display()
        {
            //these lists allow for testing if values are returned (List<Buyt>), used in the foreach to get data(List<Base>) or
            //are used to return all results(List<BudgBuyVehViewModel>)
            List<BudgRentVehViewModel> BRVData = new List<BudgRentVehViewModel>();
            List<Expenses> expenses = new List<Expenses>();
            List<Base> bases = db.Base.ToList();
            string User = HttpContext.Session.GetString("LoggedInUser");
            //This if statement will make it so that the page cannot be accessed unless a user is logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                //foreach makes code inside apply to every item within list<Base>
                foreach (Base item in bases)
                {
                    BudgRentVehViewModel BudgetRentVeh = new BudgRentVehViewModel();
                    BudgetRentVeh.Base = item;
                    //this code will match what evers in table Rent with what evers in table Base based on the username in both
                    Rent rent = db.Rent.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    BudgetRentVeh.Rent = rent;

                    Vehicle vehicle = db.Vehicle.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    BudgetRentVeh.Vehicle = vehicle;
                    BRVData.Add(BudgetRentVeh);

                    //these lines of code are the parts that take in the data from the tables where the values are
                    //and then sends them to the class library where they are put into calculations and the result is then sent back
                    //and added to the custome table made
                    Expenses expense = new Expenses(item.Groceries, item.WaterLights, item.Travel, item.Phone, item.Other);
                    BudgetRentVeh.Expense = (decimal)expense.Expenses_Total();

                    Taxes tax = new Taxes((int)item.Income);
                    BudgetRentVeh.Taxes = Math.Round((double)tax.Tax_Total(), 2);

                    Vehicle_expenses veh = new Vehicle_expenses((double)vehicle.PurPrice, (double)vehicle.Deposit, (double)vehicle.Interest, (double)vehicle.Insurance);
                    BudgetRentVeh.VehicleCost = Math.Round((double)veh.Vehicle_total(), 2);

                    //this final part of code gets the values that we already have as well as the calculation results and then
                    //puts them through another calculation that will give us the result of how much money is left
                    BudgetRentVeh.Remaining = ((double)item.Income - (double)BudgetRentVeh.Expense - BudgetRentVeh.Taxes - (double)rent.ReantAmnt - BudgetRentVeh.VehicleCost);
                }
                return View(BRVData.Where(c => c.Base.UserName.Equals(User)));
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }
}
