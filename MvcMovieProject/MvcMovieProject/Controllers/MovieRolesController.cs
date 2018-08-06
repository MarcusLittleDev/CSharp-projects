using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovieProject.Data;
using MvcMovieProject.Models;

namespace MvcMovieProject.Controllers
{
    public class MovieRolesController : Controller
    {
        private readonly MvcMovieProjectContext _context;

        public MovieRolesController(MvcMovieProjectContext context)
        {
            _context = context;
        }

        // GET: MovieRoles
        public async Task<IActionResult> Index()
        {
            var mvcMovieProjectContext = _context.MovieRole.Include(m => m.Actor).Include(m => m.Movie);
            return View(await mvcMovieProjectContext.ToListAsync());
        }

        // GET: MovieRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRole = await _context.MovieRole
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .SingleOrDefaultAsync(m => m.MovieRoleID == id);
            if (movieRole == null)
            {
                return NotFound();
            }

            return View(movieRole);
        }

        // GET: MovieRoles/Create
        public IActionResult Create()
        {
            ViewData["ActorID"] = new SelectList(_context.Actor, "ActorID", "Name");
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Genre");
            return View();
        }

        // POST: MovieRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieRoleID,ActorID,MovieID,Character")] MovieRole movieRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorID"] = new SelectList(_context.Actor, "ActorID", "Name", movieRole.ActorID);
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Genre", movieRole.MovieID);
            return View(movieRole);
        }

        // GET: MovieRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRole = await _context.MovieRole.SingleOrDefaultAsync(m => m.MovieRoleID == id);
            if (movieRole == null)
            {
                return NotFound();
            }
            ViewData["ActorID"] = new SelectList(_context.Actor, "ActorID", "Name", movieRole.ActorID);
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Genre", movieRole.MovieID);
            return View(movieRole);
        }

        // POST: MovieRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieRoleID,ActorID,MovieID,Character")] MovieRole movieRole)
        {
            if (id != movieRole.MovieRoleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieRoleExists(movieRole.MovieRoleID))
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
            ViewData["ActorID"] = new SelectList(_context.Actor, "ActorID", "Name", movieRole.ActorID);
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Genre", movieRole.MovieID);
            return View(movieRole);
        }

        // GET: MovieRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRole = await _context.MovieRole
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .SingleOrDefaultAsync(m => m.MovieRoleID == id);
            if (movieRole == null)
            {
                return NotFound();
            }

            return View(movieRole);
        }

        // POST: MovieRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieRole = await _context.MovieRole.SingleOrDefaultAsync(m => m.MovieRoleID == id);
            _context.MovieRole.Remove(movieRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieRoleExists(int id)
        {
            return _context.MovieRole.Any(e => e.MovieRoleID == id);
        }
    }
}
