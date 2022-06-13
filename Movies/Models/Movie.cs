using Microsoft.AspNetCore.Http;
using Movies.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string PosterImagePath { get; set; }

        [MaxLength(5000)]
        public string Overview { get; set; }
        [Required]
        [Range(0,10)]
        public double Score { get; set; }

        [Display(Name = "Upload Image : ")]
        [NotMapped]
        [ValidImage]
        public IFormFile PosterToUpload { get; set; }
    }
}
