using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcMovieProject.Models.MovieViewModels
{
    public class ActorIndexViewData
    {
        public IEnumerable<Movie> Movies { get; set; }
        public Actor Actor { get; set; }
        public IEnumerable<MovieRole> MovieRoles { get; set; }
    }
}
