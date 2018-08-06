using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcMovieProject.Models;

namespace MvcMovieProject.MovieViewModels.Models
{
    public class MovieIndexData
    {
        public int ID { get; set; }
        public Movie Movie {get; set;}
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<MovieRole> MovieRoles { get; set; }
    }
}
