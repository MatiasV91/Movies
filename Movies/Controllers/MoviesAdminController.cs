using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;

namespace Movies.Controllers
{
    public class MoviesAdminController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

        public MoviesAdminController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: MoviesAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _movieRepository.GetMoviesAsync());
        }

        // GET: MoviesAdmin/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: MoviesAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MoviesAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (movie.PosterToUpload != null)
            {
                ValidateIMGUpload(movie);
            }
            if (ModelState.IsValid)
            {
                await _movieRepository.AddAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }


      
        // GET: MoviesAdmin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: MoviesAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Movie movie)
        {
            if (movie.PosterToUpload != null)
            {
                ValidateIMGUpload(movie);
            }

            if (ModelState.IsValid)
            {
                await _movieRepository.UpdateAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: MoviesAdmin/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: MoviesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
                return NotFound();
            await _movieRepository.DeleteAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        private void ValidateIMGUpload(Movie movie)
        {
            var ext = Path.GetExtension(movie.PosterToUpload.FileName).ToLowerInvariant();
            if (!permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError(nameof(movie.PosterToUpload), "Only supported Image formats are JPG and PNG");
            }
        }
    }
}
