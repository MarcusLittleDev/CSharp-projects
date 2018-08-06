using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovieProject.Models;

namespace MvcMovieProject.Data
{
    public class MvcMovieProjectContext : DbContext
    {
        public MvcMovieProjectContext (DbContextOptions<MvcMovieProjectContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<MovieRole> MovieRole { get; set; }
    }
}
