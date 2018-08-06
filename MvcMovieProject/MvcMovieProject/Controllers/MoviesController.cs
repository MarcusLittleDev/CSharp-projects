using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovieProject.Data;
using MvcMovieProject.Models;
using MvcMovieProject.MovieViewModels.Models;

namespace MvcMovieProject.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieProjectContext _context;

        public MoviesController(MvcMovieProjectContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int? id, string movieGenre, string movieSearch, string actorSearch, string sortOrder, string actorFilter, string movieFilter, int? page)
        {

            ViewData["CurrentSort"] = sortOrder;

            if (actorSearch != null || movieSearch != null)
            {
                page = 1;
            }
            else
            {
                actorSearch = actorFilter;
                movieSearch = movieFilter;
            }
            ViewData["ActorFilter"] = actorSearch;
            ViewData["MovieFilter"] = movieSearch;
            ViewData["CurrentPage"] = page;


            var viewModel = new MovieIndexData();

            viewModel.Movies = await _context.Movie
                .Include(i => i.MovieRoles)
                    .ThenInclude(i => i.Actor)
                .AsNoTracking()
                .ToListAsync();

            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;
            var actors = from a in _context.Actor
                         select a;
            var movieRoles = from mr in _context.MovieRole
                             select mr;

            if (!String.IsNullOrEmpty(movieSearch) & !String.IsNullOrEmpty(actorSearch))
            {
                movieRoles = movieRoles.Where(s => ((s.Actor.Name).Contains(actorSearch) || (s.Actor.BirthName).Contains(actorSearch)) & 
                                            s.Movie.Title.Contains(movieSearch));
                movies = movies.Where(s => movieRoles.Any(s2 => s2.MovieID == s.ID));
            }
            else if (!String.IsNullOrEmpty(movieSearch))
            {
                movies = movies.Where(s => s.Title.Contains(movieSearch));
            }

            else if (!String.IsNullOrEmpty(actorSearch))
            {
                movieRoles = movieRoles.Where(s => ((s.Actor.Name).Contains(actorSearch) || (s.Actor.BirthName).Contains(actorSearch)));
                movies = movies.Where(s => movieRoles.Any(s2 => s2.MovieID == s.ID));

            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            if (id != null)
            {
                ViewData["MovieID"] = id.Value;
                Movie movie = viewModel.Movies.Where(
                    i => i.ID == id.Value).Single();
                viewModel.Actors = movie.MovieRoles.Select(s => s.Actor).Distinct();
                viewModel.MovieRoles = movie.MovieRoles;
                viewModel.ID = (int)id;
                
            }

            movies = Sort(sortOrder, movies);
            var movieGenreVM = new MovieGenreViewModel();
            movieGenreVM.genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            movieGenreVM.MovieIndexData = viewModel;
            int pageSize = 7;
            var pList = await PaginatedList<Movie>.CreateAsync(movies, page ?? 1, pageSize);
            movieGenreVM.PageList = pList;



            return View(movieGenreVM);
        }

        //Orders the movies by the chosen parameter
        private IQueryable<Movie> Sort(string sortOrder, IQueryable<Movie> movies)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Release Date" ? "date_desc" : "Release Date";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";         

            switch (sortOrder)
            {
                case "title_desc":
                    {
                        movies = movies.OrderByDescending(s => s.Title);
                    }
                    break;
                case "Release Date":
                    {
                        movies = movies.OrderBy(s => s.ReleaseDate);
                    }
                    break;
                case "date_desc":
                    {
                        movies = movies.OrderByDescending(s => s.ReleaseDate);
                    };
                    break;
                case "Price":
                    {
                        movies = movies.OrderBy(s => s.Price);
                    }
                    break;
                case "price_desc":
                    {
                        movies = movies.OrderByDescending(s => s.Price);
                    }
                    break;
                case "Genre":
                    {
                        movies = movies.OrderBy(s => s.Genre);
                    }
                    break;
                case "Genre_desc":
                    {
                        movies = movies.OrderByDescending(s => s.Genre);
                    }
                    break;
                case "Rating":
                    {
                        movies = movies.OrderBy(s => s.Rating);
                    }
                    break;
                case "rating_desc":
                    {
                        movies = movies.OrderByDescending(s => s.Rating);
                    }
                    break;
                default:
                    {
                        movies = movies.OrderBy(s => s.Title);
                    }
                    break;
            }
            return movies;
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            MovieIndexData viewModel = new MovieIndexData();
            var actors = from role in _context.MovieRole
                         where role.MovieID == id
                         select role.Actor;
            var movieRoles = from role in _context.MovieRole
                             where role.MovieID == id
                             select role;
            viewModel.Movie = movie;
            viewModel.ID = (int)id; 
            viewModel.Actors = actors;
            viewModel.MovieRoles = movieRoles;

            return View(viewModel);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,ReleaseDate,Genre,Price,Rating,Image")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            MovieIndexData viewModel = new MovieIndexData();
            var actors = from role in _context.MovieRole
                         where role.MovieID == id
                         select role.Actor;
            var movieRoles = from role in _context.MovieRole
                        where role.MovieID == id
                        select role;
            viewModel.Actors = actors;
            viewModel.Movie = movie;
            viewModel.MovieRoles = movieRoles;
            return View(viewModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Genre,Price,Rating,Image")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? roleID, bool? isRole)
        {

                var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public JsonResult GetModel()
        {
            var model = (from actor in _context.MovieRole
                         orderby actor.Actor.Name
                         select actor.Actor).Distinct().ToList();

            return Json(model);
        }

        public async Task<JsonResult> SaveRow([Bind("MovieRoleID,ActorID,MovieID,Character")] MovieRole movieRole)
        {

            if (ModelState.IsValid)
            {

                _context.Add(movieRole);
                await _context.SaveChangesAsync();


            }

            var query = await (from actor in _context.Actor
                               where actor.ActorID == movieRole.ActorID
                               select actor).FirstOrDefaultAsync();

            IDictionary<string, string> jsonObject = new Dictionary<string, string>()
            {
                {"Name", query.Name },
                {"Character", movieRole.Character },
                {"MovieRoleID", movieRole.MovieRoleID.ToString() }
            };

            return Json(jsonObject);
        }


        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
