using MovieShop.Core.Entities;
using MovieShop.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IMovieRepository : IAsyncRepository<Movie>
    {
        //Task<IEnumerable<Movie>> GetTopRatedMovies();
        Task<IEnumerable<MovieRatingResponseModel>> GetTopRatedMovies();
        Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId);
        Task<IEnumerable<Movie>> GetHighestRevenueMovies();
        Task<Movie> GetMovieByTitle(string movieTitle);
        Task<IEnumerable<Review>> GetMovieReviews(int id);
    }
}
