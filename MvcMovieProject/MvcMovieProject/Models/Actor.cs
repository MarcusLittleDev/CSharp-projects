using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcMovieProject.Models
{
    public class Actor
    {
        public int ActorID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string Name { get; set; }


        [Display(Name = "Birth Date"), DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Home Town")]
        public string HomeTown { get; set; }

        [Display(Name = "Birth Name")]
        public string BirthName { get; set; }

        public ICollection<MovieRole> MovieRoles { get; set; }
    }
}
