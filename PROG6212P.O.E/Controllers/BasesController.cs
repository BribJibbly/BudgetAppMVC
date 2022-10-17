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
    public class BasesController : Controller
    {
        private readonly STORAGETASK3Context _context;

        public BasesController(STORAGETASK3Context context)
        {
            _context = context;
        }

        // GET: Bases
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    var sTORAGETASK3Context = _context.Base.Include(a => a.UserNameNavigation);
                    return View(await sTORAGETASK3Context.ToListAsync());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login","Login");
            }
        }

        // GET: Bases/Details/5
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

                    var @base = await _context.Base
                        .Include(a
                        => a.UserNameNavigation)
                        .FirstOrDefaultAsync(m => m.UserName == id);
                    if (@base == null)
                    {
                        return NotFound();
                    }

                    return View(@base);
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

        // GET: Bases/Create
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

        // POST: Bases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Income,Tax,Groceries,WaterLights,Travel,Phone,Other")] Base @base)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@base);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", @base.UserName);
            return View(@base);
        }

        // GET: Bases/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @base = await _context.Base.FindAsync(id);
            if (@base == null)
            {
                return NotFound();
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", @base.UserName);
            return View(@base);
        }

        // POST: Bases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Income,Tax,Groceries,WaterLights,Travel,Phone,Other")] Base @base)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserName"] = new SelectList(_context.TblUser, "UserName", "UserName", @base.UserName);
            return View(@base);
        }

        // GET: Bases/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @base = await _context.Base
                .Include(a => a.UserNameNavigation)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (@base == null)
            {
                return NotFound();
            }

            return View(@base);
        }

        // POST: Bases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @base = await _context.Base.FindAsync(id);
            _context.Base.Remove(@base);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaseExists(string id)
        {
            return _context.Base.Any(e => e.UserName == id);
        }
    }
}
