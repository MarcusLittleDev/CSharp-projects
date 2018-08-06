using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovieProject.Models
{
    public class MovieRole
    {
        public int MovieRoleID { get; set; }
        public int ActorID {get; set;}
        public int MovieID { get; set; }
        [StringLength(60)]
        [Required]
        public string Character { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
    }

    public class JsonRole
    {
        public int ActorID { get; set; }
        public int MovieID { get; set; }
        [Required]
        public string Character { get; set; }
    }
}
