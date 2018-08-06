using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovieProject.Models;

namespace MvcMovieProject.MovieViewModels.Models
{
    public class MovieGenreViewModel
    {
        public PaginatedList<Movie> PageList { get; set; }
        public List<Movie> movies;
        public SelectList genres;
        public string movieGenre { get; set; }
        public MovieIndexData MovieIndexData { get; set; }
    }
}
