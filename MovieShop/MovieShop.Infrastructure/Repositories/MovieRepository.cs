using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Repositories
{
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
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
            var movies = await _dbContext.MovieGenre.Where(g => g.GenreId == genreId)
                .Include(mg => mg.Movie)
                .Select(m => m.Movie).ToListAsync();
            return movies;
        }
        public async Task<Movie> GetMovieByTitle(string movieTitle)
        {
            return await _dbContext.Movie.FirstOrDefaultAsync(m => m.Title == movieTitle);
        }
        public async Task<IEnumerable<MovieRatingResponseModel>> GetTopRatedMovies()
        {
            var topRatedMovies = await _dbContext.Review.Include(m => m.Movie)
                                                 .GroupBy(r => new
                                                 {
                                                     Id = r.MovieId,
                                                     r.Movie.PosterUrl,
                                                     r.Movie.Title,
                                                     r.Movie.ReleaseDate
                                                 })
                                                 .OrderByDescending(g => g.Average(m => m.Rating))
                                                 .Select(m => new MovieRatingResponseModel
                                                 {
                                                     Id = m.Key.Id,
                                                     PosterUrl = m.Key.PosterUrl,
                                                     Title = m.Key.Title,
                                                     ReleaseDate = m.Key.ReleaseDate,
                                                     Rating = m.Average(x => x.Rating)
                                                 })
                                                 .Take(50)
                                                 .ToListAsync();

            return topRatedMovies;
        }
        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movie
                                        .Include(m => m.MovieCast).ThenInclude(m => m.Cast).Include(m => m.MovieGenre)
                                        .ThenInclude(m => m.Genre)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return null;
            var movieRating = await _dbContext.Review.Where(r => r.MovieId == id).DefaultIfEmpty()
                                              .AverageAsync(r => r == null ? 0 : r.Rating);
            //if (movieRating > 0) movie.Rating = movieRating;
            return movie;
        }

        //public async Task<IEnumerable<Movie>> GetTopRatedMovies()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
