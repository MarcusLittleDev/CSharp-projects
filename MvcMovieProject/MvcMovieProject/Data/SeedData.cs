using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using MvcMovieProject.Models;
using CsvHelper;
using System.IO;

namespace MvcMovieProject.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieProjectContext(
                serviceProvider.GetRequiredService<DbContextOptions<MvcMovieProjectContext>>()))
            {

                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                using (CsvReader csv = new CsvReader(new StreamReader("K:/NRL/Systems/Project/littlema/MvcMovieProject/MvcMovieProject/Data/SeedData/Movies.txt"), true))
                {
                    csv.Configuration.Delimiter = "|";
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.MissingFieldFound = null;
                    var movies = csv.GetRecords<Movie>();
                    foreach (var movie in movies)
                    {
                        context.Movie.Add(movie);
                    }

                }
                context.SaveChanges();

                using (CsvReader csv = new CsvReader(new StreamReader("K:/NRL/Systems/Project/littlema/MvcMovieProject/MvcMovieProject/Data/SeedData/Actors.txt"), true))
                {
                    csv.Configuration.Delimiter = "|";
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.MissingFieldFound = null;
                    var actors = csv.GetRecords<Actor>();
                    foreach (var actor in actors)
                    {
                        context.Actor.Add(actor);
                    }

                }
                context.SaveChanges();

                using (CsvReader csv = new CsvReader(new StreamReader("K:/NRL/Systems/Project/littlema/MvcMovieProject/MvcMovieProject/Data/SeedData/MovieRoles.txt"), true))
                {
                    var anon = new
                    {
                       Actor = string.Empty,
                       Character = string.Empty,
                       Movie = string.Empty
                    };

                    csv.Configuration.Delimiter = "|";
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.MissingFieldFound = null;
                    var roles = csv.GetRecords(anon);
                    foreach (var role in roles)
                    {
                        var movieID = context.Movie.Where(m => m.Title == role.Movie).FirstOrDefault();
                        var actorID = context.Actor.Where(a => a.Name == role.Actor).FirstOrDefault();
                        context.MovieRole.Add(new MovieRole
                        {
                            MovieID = movieID.ID,
                            ActorID = actorID.ActorID,
                            Character = role.Character

                        });
                    }

                }
                context.SaveChanges();
            }
        }
    }
}
