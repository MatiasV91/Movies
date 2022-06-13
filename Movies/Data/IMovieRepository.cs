using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Data
{
    public interface IMovieRepository
    {
        Task AddAsync(Movie movie);
        Task<IEnumerable<Movie>> GetMoviesAsync(string searchString = null);
        Task DeleteAsync(Movie movie);
        Task<Movie> GetAsync(int id);
        Task UpdateAsync(Movie movie);

    }
}
