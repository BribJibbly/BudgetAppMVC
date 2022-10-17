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
    public class RentsController : Controller
    {
        private readonly STORAGETASK3Context _context;

        public RentsController(STORAGETASK3Context context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            //this if statement will prevent you entrance if you are not logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    var sTORAGETASK3Context = _context.Rent.Include(r => r.UserNameNavigation);
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

        // GET: Rents/Details/5
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

                         var rent = await _context.Rent
                           .Include(r => r.UserNameNavigation)
                           .FirstOrDefaultAsync(m => m.UserName == id);

                    if (rent == null)
                    {
                        return NotFound();
                    }

                    return View(rent);
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

        // GET: Rents/Create
        public IActionResult Create()
        {
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

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,ReantAmnt")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", rent.UserName);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", rent.UserName);
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,ReantAmnt")] Rent rent)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", rent.UserName);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .Include(r => r.UserNameNavigation)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var rent = await _context.Rent.FindAsync(id);
            _context.Rent.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(string id)
        {
            return _context.Rent.Any(e => e.UserName == id);
        }
    }
}
