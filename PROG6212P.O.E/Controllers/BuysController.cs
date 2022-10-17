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
    public class BuysController : Controller
    {
        private readonly STORAGETASK3Context _context;

        public BuysController(STORAGETASK3Context context)
        {
            _context = context;
        }

        // GET: Buys
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    var sTORAGETASK3Context = _context.Buy.Include(b => b.UserNameNavigation);
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

        // GET: Buys/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //This if statement will make it so that the page cannot be accessed unless a user is logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    if (id == null)
                    {
                         return NotFound();
                    }

                    var buy = await _context.Buy
                         .Include(b => b.UserNameNavigation)
                         .FirstOrDefaultAsync(m => m.UserName == id);
                if (buy == null)
                {
                    return NotFound();
                }

                    return View(buy);
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

        // GET: Buys/Create
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

        // POST: Buys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PropPrice,Deposit,Interest,RepayMonths")] Buy buy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", buy.UserName);
            return View(buy);
        }

        // GET: Buys/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buy = await _context.Buy.FindAsync(id);
            if (buy == null)
            {
                return NotFound();
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", buy.UserName);
            return View(buy);
        }

        // POST: Buys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", buy.UserName);
            return View(buy);
        }

        // GET: Buys/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buy = await _context.Buy
                .Include(b => b.UserNameNavigation)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (buy == null)
            {
                return NotFound();
            }

            return View(buy);
        }

        // POST: Buys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var buy = await _context.Buy.FindAsync(id);
            _context.Buy.Remove(buy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuyExists(string id)
        {
            return _context.Buy.Any(e => e.UserName == id);
        }
    }
}
