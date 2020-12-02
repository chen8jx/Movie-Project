using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Repositories
{
    public class MovieRepository: EfRepository<Movie>,IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Movie>> GetHighestRevenueMovies()
        {
            var movies = await _dbContext.Movie.OrderByDescending(m => m.Revenue).Take(50).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {
            throw new NotImplementedException();
        }
        public async Task<Movie> GetMovieByTitle(string movieTitle)
        {
            return await _dbContext.Movie.FirstOrDefaultAsync(m => m.Title == movieTitle);
        }
        public async Task<IEnumerable<Movie>> GetTopRatedMovies()
        {
            throw new NotImplementedException();
        }
        public override async Task<Movie> GetByIdAsync(int id)
        {
            //var movie = await _dbContext.Movie
            //                            .Include(m => m.MovieCast).ThenInclude(m => m.Cast).Include(m => m.MovieGenre)
            //                            .ThenInclude(m => m.Genre)
            //                            .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null) return null;
            //var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id).DefaultIfEmpty()
            //                                  .AverageAsync(r => r == null ? 0 : r.Rating);
            //if (movieRating > 0) movie.Rating = movieRating;
            //return movie;
            return null;
        }
    }
}
