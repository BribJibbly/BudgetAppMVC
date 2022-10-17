using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG6212P.O.E.Models;
using PROG6212P.O.E.ViewModels;

namespace PROG6212P.O.E.Controllers
{
    public class TablesController : Controller
    {
        private readonly STORAGETASK3Context _context;

        public TablesController(STORAGETASK3Context context)
        {
            _context = context;
        }

        // GET: Tables
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    var sTORAGETASK3Context = _context.Base.Include(a => a.UserNameNavigation);
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Tables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var tables = await _context.Tables
                        .FirstOrDefaultAsync(m => m.id == id);
                    if (tables == null)
                    {
                        return NotFound();
                    }

                    return View(tables);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        // GET: Tables/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                    
                    ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName");
                    return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        // POST: Tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBase([Bind("UserName,Income,Tax,Groceries,WaterLights,Travel,Phone,Other")] Base @base)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(@base);
                await _context.SaveChangesAsync();
                Thread.Sleep(7000);
                return RedirectToAction("Privacy", "Home");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", @base.UserName);
            return View(@base);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRent([Bind("UserName,ReantAmnt")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                Thread.Sleep(7000);
                return RedirectToAction("Index", "Bases");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", rent.UserName);
            return View(rent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle([Bind("UserName,Model,PurPrice,Deposit,Interest,Insurance")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                Thread.Sleep(7000);
                return RedirectToAction("Index", "Bases");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", vehicle.UserName);
            return View(vehicle);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBuy([Bind("UserName,PropPrice,Deposit,Interest,RepayMonths")] Buy buy,Vehicle veh,Rent rent)
        {
            decimal num = 0;
            if (ModelState.IsValid)
            {
                _context.Add(buy);
                await _context.SaveChangesAsync();
                if (buy.PropPrice == num && buy.Interest == 0 && buy.RepayMonths == 0 && buy.Deposit == num)
                {
                    return RedirectToAction("Display", "BudgRentVeh");
                }
                else if (buy.PropPrice == num && buy.Interest == 0 && buy.RepayMonths == 0 && buy.Deposit == num && veh.Deposit == num && veh.Insurance == num && veh.Interest == 0 && veh.PurPrice == num)
                {
                    return RedirectToAction("Display", "BudgRent");
                }
                else if (rent.ReantAmnt == num)
                {
                    return RedirectToAction("Display", "BudgBuyVeh");
                }
                else if (rent.ReantAmnt == num && veh.Deposit == num && veh.Insurance == num && veh.Interest == 0 && veh.PurPrice == num)
                {
                    return RedirectToAction("Display", "BudgBuy");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", buy.UserName);
            return View(buy);
        }

        // GET: Tables/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
           var tables = await _context.Base.FindAsync(id);
           if (tables == null)
           {
              return NotFound();
           }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", tables.UserName);
            return View(tables);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBase(string id, [Bind("UserName,Income,Tax,Groceries,WaterLights,Travel,Phone,Other")] Base @base)
        {
            if (id != @base.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@base);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseExists(@base.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Thread.Sleep(7000);
                return RedirectToAction("Login","Login");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", @base.UserName);
            return View(@base);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRent(string id, [Bind("UserName,ReantAmnt")] Rent rent)
        {
            if (id != rent.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Thread.Sleep(7000);
                return RedirectToAction("Login", "Login");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", rent.UserName);
            return View(rent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicle(string id, [Bind("UserName,Model,PurPrice,Deposit,Interest,Insurance")] Vehicle vehicle)
        {
            if (id != vehicle.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Thread.Sleep(7000);
                return RedirectToAction("Login", "Login");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", vehicle.UserName);
            return View(vehicle);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,PropPrice,Deposit,Interest,RepayMonths")] Buy buy)
        {
            if (id != buy.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyExists(buy.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Login", "Login");
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", buy.UserName);
            return View(buy);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id")] Tables tables)
        {
            if (id != tables.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tables);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TablesExists(tables.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Login", "Login");
            }
            return View(tables);
        }

        // GET: Tables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tables = await _context.Tables
                .FirstOrDefaultAsync(m => m.id == id);
            if (tables == null)
            {
                return NotFound();
            }

            return View(tables);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tables = await _context.Tables.FindAsync(id);
            _context.Tables.Remove(tables);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TablesExists(int id)
        {
            return _context.Tables.Any(e => e.id == id);
        }
        private bool BaseExists(string id)
        {
            return _context.Base.Any(e => e.UserName == id);
        }
        private bool RentExists(string id)
        {
            return _context.Rent.Any(e => e.UserName == id);
        }
        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.UserName == id);
        }
        private bool BuyExists(string id)
        {
            return _context.Buy.Any(e => e.UserName == id);
        }
    }
}
