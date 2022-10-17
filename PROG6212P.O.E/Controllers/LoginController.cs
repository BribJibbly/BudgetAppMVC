using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROG6212P.O.E.Models;

namespace PROG6212P.O.E.Controllers
{
    public class LoginController : Controller
    {
        //this is the context needed to access the database
        STORAGETASK3Context db = new STORAGETASK3Context();
        public LoginController(STORAGETASK3Context context)
        {
            db = context;
        }
        //this allows the web page to appear
        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }
        //this posts results based on if the username exists in the database or not 
        [HttpPost]
        public IActionResult Login(String username, string password)
        {
            //this hashes the login password so that it can match the already hashed password
            var hash = Crypto.Hash(password);
            password = hash;
            decimal num = 0;
            TblUser u = db.TblUser.Where(us => us.UserName.Equals(username) && us.Password.Equals(password)).FirstOrDefault();
            Base BaseValue = db.Base.Where(ub => ub.UserName.Equals(username)).FirstOrDefault();
            //these statments will only rteturn something thats not null if the values in their tables are zero.
            //therefore allowing us to see where the user needs to go
            Vehicle VehValue = db.Vehicle.Where(uv => uv.UserName.Equals(username) && uv.Insurance.Equals(num) && uv.Interest.Equals(0) && uv.PurPrice.Equals(num)).FirstOrDefault();
            Rent RenValue = db.Rent.Where(ur => ur.UserName.Equals(username) && ur.ReantAmnt.Equals(num)).FirstOrDefault();
            Buy BuyValue = db.Buy.Where(uby => uby.UserName.Equals(username) && uby.Deposit.Equals(num) && uby.Interest.Equals(0) && uby.PropPrice.Equals(num) && uby.RepayMonths.Equals(0)).FirstOrDefault();
            if(u != null)
            {
                HttpContext.Session.SetString("LoggedInUser", u.UserName);
                //This if statement will send you to a specific page based on what data you have in the database
                if (BaseValue == null) 
                { 
                    return RedirectToAction("Create", "Tables"); 
                }

                else
                {
                    if (VehValue == null && RenValue == null && BuyValue != null) 
                    {
                        return RedirectToAction("Display","BudgRentVeh");

                    }
                    else if (VehValue == null && RenValue != null && BuyValue == null) 
                    {
                        return RedirectToAction("Display", "BudgBuyVeh");
                    }
                    else if (VehValue != null && RenValue == null && BuyValue != null) 
                    {
                        return RedirectToAction("Display", "BudgRent");
                    }
                    else if (VehValue != null && RenValue != null && BuyValue == null) 
                    {
                        return RedirectToAction("Display", "BudgBuy");
                    }
                    else if (username != "Alkemix" && VehValue != null && RenValue != null && BuyValue != null)
                    {
                        return RedirectToAction("Display", "BudgBuy");
                    }
                    else 
                    {
                        return RedirectToAction("Privacy", "Home");
                    }
                }

            }

            
            else
            {
                ViewBag.Error = "Either the Username or Password entered are incorrect";
                return View();
            }
        }
    }
}
