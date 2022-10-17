using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PROG6212P.O.E.Models;


namespace PROG6212P.O.E.Controllers
{
    public class TblUsersController : Controller
    {
        private readonly STORAGETASK3Context _context;

        public TblUsersController(STORAGETASK3Context context)
        {
            _context = context;
        }

        // GET: TblUsers
        public async Task<IActionResult> Index()
        {
            //this if statement will prevent you entrance if you are not logged in
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                if (HttpContext.Session.GetString("LoggedInUser") == "Alkemix")
                {
                    return View(await _context.TblUser.ToListAsync());
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

        // GET: TblUsers/Details/5
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

                    var tblUser = await _context.TblUser
                        .FirstOrDefaultAsync(m => m.UserName == id);
                    if (tblUser == null)
                    {
                        return NotFound();
                    }

                    return View(tblUser);
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

        // GET: TblUsers/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("LoggedInUser") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        // POST: TblUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password")] TblUser tblUser)
        {
            int u = _context.TblUser.Where(x => x.UserName.ToLower().Equals(tblUser.UserName.ToLower())).Count();
            if (u == 0)
            {
                if (ModelState.IsValid)
                {
                    //this hashes the newly created password
                    var hash = Crypto.Hash(tblUser.Password);
                    tblUser.Password = hash;
                    _context.Add(tblUser);
                    await _context.SaveChangesAsync();
                    ViewBag.Create = "User has been succesfully registered";
                    return RedirectToAction("Login","Login");

                }
                return View(tblUser);

            }
            else
            {
                ViewBag.Exist = "Entered username already exists";
                return View(tblUser);
            }
           
        }

        // GET: TblUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUser.FindAsync(id);
            if (tblUser == null)
            {
                return NotFound();
            }
            return View(tblUser);
        }

        // POST: TblUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Password")] TblUser tblUser)
        {
            if (id != tblUser.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUserExists(tblUser.UserName))
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
            return View(tblUser);
        }

        // GET: TblUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUser
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (tblUser == null)
            {
                return NotFound();
            }

            return View(tblUser);
        }

        // POST: TblUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);
            _context.TblUser.Remove(tblUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblUserExists(string id)
        {
            return _context.TblUser.Any(e => e.UserName == id);
        }
    }
}
