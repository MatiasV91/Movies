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
        private readonly ApplicationDbContext _context;
        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

        public MoviesAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoviesAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: MoviesAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
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
                if (movie.PosterToUpload != null)
                {
                    string filename = await SaveImage(movie.PosterToUpload);
                    movie.PosterImagePath = filename;
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }


      
        // GET: MoviesAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
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
                try
                {
                    if (movie.PosterToUpload != null)
                    {
                        if(!String.IsNullOrEmpty(movie.PosterImagePath))
                            DeleteImage(movie.PosterImagePath);
                        string filename = await SaveImage(movie.PosterToUpload);
                        movie.PosterImagePath = filename;
                    }
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: MoviesAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var movie = await _context.Movies.FindAsync(id);
            DeleteImage(movie.PosterImagePath);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }


        private void ValidateIMGUpload(Movie movie)
        {
            var ext = Path.GetExtension(movie.PosterToUpload.FileName).ToLowerInvariant();
            if (!permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError(nameof(movie.PosterToUpload), "Only supported Image formats are JPG and PNG");
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var filename = Guid.NewGuid().ToString() + ext;
            var path = Path.GetFullPath("./wwwroot/img/") + filename;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
        }

        private void DeleteImage(string filename)
        {
            var path = Path.GetFullPath("./wwwroot/img/") + filename;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
