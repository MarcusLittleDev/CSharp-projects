using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovieProject.Data;
using MvcMovieProject.Models;
using MvcMovieProject.Models.MovieViewModels;

namespace MvcMovieProject.Controllers
{
    public class ActorsController : Controller
    {
        private readonly MvcMovieProjectContext _context;

        public ActorsController(MvcMovieProjectContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LocationSortParm"] = sortOrder == "Home Town" ? "location_desc" : "Home Town";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var actors = from a in _context.Actor
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                actors = actors.Where(s => (s.Name).Contains(searchString));
            }

            actors = Sort(sortOrder, actors);

            int pageSize = 10; 
            return View(await PaginatedList<Actor>.CreateAsync(actors.AsNoTracking(), page ?? 1, pageSize));
        }

        private IQueryable<Actor> Sort(string sortOrder, IQueryable<Actor> actors)
        {
            switch(sortOrder)
            {
                case "name_desc":
                    actors = actors.OrderByDescending(s => s.Name);
                    break;
                case "Home Town":
                    actors = actors.OrderBy(s => s.HomeTown);
                    break;
                case "location_desc":
                    actors = actors.OrderByDescending(s => s.HomeTown);
                    break;
                default:
                    actors = actors.OrderBy(s => s.Name);
                    break;
            }

            return (actors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .SingleOrDefaultAsync(m => m.ActorID == id);
            if (actor == null)
            {
                return NotFound();
            }

            ActorIndexViewData viewModel = new ActorIndexViewData();
            var movies = from role in _context.MovieRole
                             where role.ActorID == id
                             select role.Movie;
            var movieRoles = from role in _context.MovieRole
                             where role.ActorID == id
                             select role;
            viewModel.Movies = movies;
            viewModel.Actor = actor;
            viewModel.MovieRoles = movieRoles;

            return View(viewModel);
        }
    

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorID,Name, BirthName, BirthDate, HomeTown")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.SingleOrDefaultAsync(m => m.ActorID == id);
            if (actor == null)
            {
                return NotFound();
            }

            ActorRoleAdditionData viewModel = new ActorRoleAdditionData();
            ActorIndexViewData actorIndexViewData = new ActorIndexViewData();
            var allMovies = from movie in _context.Movie
                            select movie.Title;
            var movies = from role in _context.MovieRole
                         where role.ActorID == id
                         select role.Movie;
            var movieRoles = from role in _context.MovieRole
                             where role.ActorID == id
                             select role;
            viewModel.Movies = new SelectList(await allMovies.ToListAsync());
            actorIndexViewData.Movies = movies;
            actorIndexViewData.Actor = actor;
            actorIndexViewData.MovieRoles = movieRoles;
            viewModel.ActorIndexViewData = actorIndexViewData;

            return View(viewModel);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorID, Name, BirthName, BirthDate, HomeTown")] Actor actor)
        {
            if (id != actor.ActorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorID))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .SingleOrDefaultAsync(m => m.ActorID == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actor.SingleOrDefaultAsync(m => m.ActorID == id);
            _context.Actor.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetModel(int id)
        {
            Console.WriteLine(id);
            var movies = (from movie in _context.Movie
                         orderby movie.Title
                        select movie).Distinct().ToList();
            var roles = (from role in _context.MovieRole
                         where role.ActorID == id
                         select role.Movie).Distinct().ToList();
            var model = movies.Where(x => !(roles.Contains(x)) == true).ToList();
            foreach(var i in model) { Console.WriteLine(i.Title); }
            return Json(model);
        }
        
        [HttpPost]
        public async Task<JsonResult> SaveRow([Bind("MovieRoleID,ActorID,MovieID,Character")] MovieRole movieRole)
        {
            
            if (ModelState.IsValid)
            {
                
                _context.Add(movieRole);
                await _context.SaveChangesAsync();


            }

            var query = await (from movie in _context.Movie
                        where movie.ID == movieRole.MovieID
                        select movie).FirstOrDefaultAsync();

            IDictionary<string, string> jsonObject = new Dictionary<string, string>()
            {
                {"Title", query.Title },
                {"Character", movieRole.Character },
                {"MovieRoleID", movieRole.MovieRoleID.ToString() }
            };

            return Json(jsonObject);
        }

        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.ActorID == id);
        }
    }
}
