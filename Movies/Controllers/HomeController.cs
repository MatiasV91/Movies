using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Data;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public HomeController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            return View(await _movieRepository.GetMoviesAsync(searchString));
        }


        public async Task<IActionResult> Detail(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }
    }
}
