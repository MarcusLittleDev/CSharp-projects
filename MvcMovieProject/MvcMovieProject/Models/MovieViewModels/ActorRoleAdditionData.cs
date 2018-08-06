using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MvcMovieProject.Models.MovieViewModels
{
    public class ActorRoleAdditionData
    {
        public ActorIndexViewData ActorIndexViewData { get; set; }
        public SelectList Movies;
        public string Movie { get; set; }
    }
}
