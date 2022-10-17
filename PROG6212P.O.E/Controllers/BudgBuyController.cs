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
    public class BudgBuyController : Controller
    {
        //database context
        STORAGETASK3Context db = new STORAGETASK3Context();
        public BudgBuyController(STORAGETASK3Context context)
        {
            db = context;
        }

        //this is the code that allows the view model to get values from two different tables and order them based on user
        public IActionResult Display()
        {

            //these lists allow for testing if values are returned (List<Rent>), used in the foreach to get data(List<Base>) or
            //are used to return all results(List<BudgRentViewModel>)
            List<BudgBuyViewModel> BBData = new List<BudgBuyViewModel>();
            List<Expenses> expenses = new List<Expenses>();
            List<Base> bases = db.Base.ToList();
            List<Buy> buys = db.Buy.ToList();
            string User = HttpContext.Session.GetString("LoggedInUser");
            //This if statement will make it so that the page cannot be accessed unless a user is logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                //foreach makes code inside apply to every item within list<Base>
                foreach (Base item in bases)
                {
                    BudgBuyViewModel BudgetBuy = new BudgBuyViewModel();
                    BudgetBuy.Base = item;
                    //this code will match what evers in table Rent with what evers in table Base based on the username in both
                    Buy buy = db.Buy.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    BudgetBuy.Buy = buy;
                    BBData.Add(BudgetBuy);
                    //these lines of code are the parts that take in the data from the tables where the values are
                    //and then sends them to the class library where they are put into calculations and the result is then sent back
                    //and added to the custome table made
                    Expenses expense = new Expenses(item.Groceries, item.WaterLights, item.Travel, item.Phone, item.Other);
                    BudgetBuy.Expense = (decimal)expense.Expenses_Total();

                    Taxes tax = new Taxes((int)item.Income);
                    BudgetBuy.Taxes = Math.Round((double)tax.Tax_Total(), 2);

                    Prop_Cost prop = new Prop_Cost((int)buy.PropPrice, (int)buy.Deposit, (int)buy.Interest, (int)buy.RepayMonths);
                    BudgetBuy.Property = prop.Property_Total();

                    //this final part of code gets the values that we already have as well as the calculation results and then
                    //puts them through another calculation that will give us the result of how much money is left
                    BudgetBuy.Remaining = ((double)item.Income - (double)BudgetBuy.Expense - BudgetBuy.Taxes - BudgetBuy.Property);
                }
                return View(BBData.Where(c => c.Base.UserName.Equals(User)));
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }
}
