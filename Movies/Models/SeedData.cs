using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Data;
using Movies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Models
{
    public static class SeedData
    {
        private const string _adminUser = "admin@example.com";
        private const string _adminPassword = "Secret123$";

        public async static void PopulateDataBase(IApplicationBuilder app)
        {

            ApplicationDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var tmdb = app.ApplicationServices.CreateScope().ServiceProvider.GetService<ITheMovieDbService>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            if (!context.Movies.Any())
            {
                var movies = await tmdb.GetPopularMovies();
                context.Movies.AddRange(movies);
                foreach(var movie in movies)
                {
                    tmdb.DownloadImage(movie.PosterImagePath);
                }
                context.SaveChanges();
            }
        }

        public async static void PopulateDataBaseAdmin(IApplicationBuilder app)
        {

            ApplicationDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var tmdb = app.ApplicationServices.CreateScope().ServiceProvider.GetService<ITheMovieDbService>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            UserManager<IdentityUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser user = await userManager.FindByEmailAsync(_adminUser);
            if(user == null)
            {
                user = new IdentityUser(_adminUser);
                user.Email = _adminUser;
                await userManager.CreateAsync(user, _adminPassword);
            }
        }
    }
}
