using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Data
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Movie movie)
        {
            if (movie.PosterToUpload != null)
            {
                string filename = await SaveImage(movie.PosterToUpload);
                movie.PosterImagePath = filename;
            }
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie movie)
        {
            DeleteImage(movie.PosterImagePath);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<Movie> GetAsync(int id)
        {
            return await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(string searchString = null)
        {
            var movies = _context.Movies.Select(m => m);
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }
            return await movies.ToListAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            if (movie.PosterToUpload != null)
            {
                if (!String.IsNullOrEmpty(movie.PosterImagePath))
                    DeleteImage(movie.PosterImagePath);
                string filename = await SaveImage(movie.PosterToUpload);
                movie.PosterImagePath = filename;
            }
            _context.Update(movie);
            await _context.SaveChangesAsync();
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
