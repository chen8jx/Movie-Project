using MovieShop.Core.Entities;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Data;
using MovieShop.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        //private MovieRepository repository is also working
        //because MR is inhenrited from IMR

        //Constructor Injection
        //DI is pattern that enables us to wriite loosely coupled code so taht code is more maintainable and testable


        public MovieService(IMovieRepository repository, IAsyncRepository<Favorite> favoriteRepository)
        {
            //create MovieRepo instance in every method in my service class
            //newing up is very convienient but we need to avoid it as much as we can
            //_repository = new MovieRepository(new MovieShopDbContext(null));
            _repository = repository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<MovieDetailsResponseModel> GetMovieAsync(int id)
        {
            var movie = await _repository.GetByIdAsync(id);
            var count = await _favoriteRepository.GetCountAsync(f => f.MovieId == id);
            var movieDetails = new MovieDetailsResponseModel
            {
                Id = movie.Id,
                Title = movie.Title,
                PosterUrl = movie.PosterUrl,
                BackdropUrl = movie.BackdropUrl,
                //Rating = movie.Rating,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                ReleaseDate = movie.ReleaseDate,
                RunTime = movie.RunTime,
                Price = movie.Price,
                FavoritesCount = count
            };
            return movieDetails;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetTopRevenueMovies()
        {
            // Repository?
            // MovieRepository class

            var movies = await _repository.GetHighestRevenueMovies();
            // Map our Movie Entity to MovieResponseModel
            var movieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate.Value,
                    Title = movie.Title
                });
            }
            return movieResponseModel;
        }
    }
}
