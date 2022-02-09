using Movies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Services
{
    public interface ITheMovieDbService
    {
        string GetImagePath { get; }

        Task<IEnumerable<Movie>> GetPopularMovies();

        void DownloadImage(string filename);
    }
}