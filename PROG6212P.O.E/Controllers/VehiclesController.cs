using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG6212P.O.E.Models;

namespace PROG6212P.O.E.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly STORAGETASK3Context _context;

        public VehiclesController(STORAGETASK3Context context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    var sTORAGETASK3Context = _context.Vehicle.Include(v => v.UserNameNavigation);
                    return View(await sTORAGETASK3Context.ToListAsync());
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

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var vehicle = await _context.Vehicle
                        .Include(v => v.UserNameNavigation)
                        .FirstOrDefaultAsync(m => m.UserName == id);
                    if (vehicle == null)
                    {
                        return NotFound();
                    }

                    return View(vehicle);
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

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            //this if statement will prevent you entrance if you are not logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName");
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

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Model,PurPrice,Deposit,Interest,Insurance")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", vehicle.UserName);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", vehicle.UserName);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Model,PurPrice,Deposit,Interest,Insurance")] Vehicle vehicle)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", vehicle.UserName);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.UserNameNavigation)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.UserName == id);
        }
    }
}
