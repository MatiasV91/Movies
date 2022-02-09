using Movies.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Movies.Services
{
    public class TheMovieDbService : ITheMovieDbService
    {
        private readonly IHttpClientFactory _clientFactory;

        public TheMovieDbService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        const string API_KEY = "1553e8b64e390f86523dfe8b3f079a4e";
        const string POPULAR_MOVIES = @"https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&api_key=" + API_KEY + "&page=1";
        const string IMAGE_PATH = @"https://image.tmdb.org/t/p/w1280";
        const string SEARCH_URL = @"https://api.themoviedb.org/3/search/movie&api_key=" + API_KEY + "&query=";

        public async Task<IEnumerable<Movie>> GetPopularMovies()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetStringAsync(POPULAR_MOVIES);
            TmDBResponse tmDBResponse = JsonSerializer.Deserialize<TmDBResponse>(response);
            return ConvertToMovies(tmDBResponse);
        }

        private IEnumerable<Movie> ConvertToMovies(TmDBResponse response)
        {
            var movies = new List<Movie>();
            foreach (var movie in response.results)
            {
                movies.Add(new Movie
                {
                    Overview = movie.overview,
                    Title = movie.title,
                    PosterImagePath = movie.poster_path,
                    Score = movie.vote_average
                });
            }
            return movies;
        }

        public string GetImagePath => IMAGE_PATH;

        public void DownloadImage(string filename)
        {
            var path = Path.GetFullPath("./wwwroot/img") + filename;
            var uri = new Uri(IMAGE_PATH + filename);
            using (var wc = new WebClient())
            {
                wc.DownloadFileAsync(uri, path);
            }
        }
    }
}
