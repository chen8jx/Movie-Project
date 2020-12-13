using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Data;
using MovieShop.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        private readonly IAsyncRepository<Cast> _castRepository;
        private readonly IAsyncRepository<Genre> _genreRepository;
        //private MovieRepository repository is also working
        //because MR is inhenrited from IMR

        //Constructor Injection
        //DI is pattern that enables us to wriite loosely coupled code so taht code is more maintainable and testable


        public MovieService(IMovieRepository movieRepository, IAsyncRepository<Favorite> favoriteRepository,
            IAsyncRepository<Cast> castRepository, IAsyncRepository<Genre> genreRepository)
        {
            //create MovieRepo instance in every method in my service class
            //newing up is very convienient but we need to avoid it as much as we can
            //_repository = new MovieRepository(new MovieShopDbContext(null));
            _movieRepository = movieRepository;
            _favoriteRepository = favoriteRepository;
            _castRepository = castRepository;
            _genreRepository = genreRepository;
        }

        public async Task<MovieDetailsResponseModel> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            var dbMovie = await _movieRepository.GetMovieByTitle(movieCreateRequest.Title);
            if (dbMovie != null && string.Equals(dbMovie.Title, movieCreateRequest.Title, StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Movie Already Exits");

            var movie = new Movie
            {
                Title = movieCreateRequest.Title,
                Overview = movieCreateRequest.Overview
            };
            var createMovie = await _movieRepository.AddAsync(movie);
            var response = new MovieDetailsResponseModel
            {
                Id = createMovie.Id,
                Title = createMovie.Title,
                Overview = createMovie.Overview
            };
            return response;
        }

        public Task<PagedResultSet<MovieResponseModel>> GetAllMoviePurchasesByPagination(int pageSize = 20, int page = 0)
        {
            throw new NotImplementedException();
        }

        //public Task<PaginatedList<MovieResponseModel>> GetAllPurchasesByMovieId(int movieId)
        //{
        //    throw new NotImplementedException();
        //}
        //public Task<PagedResultSet<MovieResponseModel>> GetMoviesByPagination(int pageSize = 20, int page = 0, string title = "")
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<MovieDetailsResponseModel> GetMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
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
            var genres = new List<Genre>();
            foreach (var movieGenre in movie.MovieGenre)
            {
                var genre = await _genreRepository.GetByIdAsync(movieGenre.GenreId);
                var response = new Genre
                {
                    Id = genre.Id,
                    Name = genre.Name
                };
                genres.Add(response);
            }
            movieDetails.Genres = genres;
            var casts = new List<MovieDetailsResponseModel.CastResponseModel>();
            foreach (var movieCast in movie.MovieCast)
            {
                var cast = await _castRepository.GetByIdAsync(movieCast.CastId);
                var response = new MovieDetailsResponseModel.CastResponseModel
                {
                    Id = cast.Id,
                    Name = cast.Name,
                    Gender = cast.Gender,
                    TmdbUrl = cast.TmdbUrl,
                    ProfilePath = cast.ProfilePath,
                    Character = movieCast.Charater
                };
                casts.Add(response);
            }
            movieDetails.Casts = casts;
            return movieDetails;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieRepository.GetMoviesByGenre(genreId);
            if (!movies.Any())
                throw new Exception("No Movies Found");
            var response = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                response.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate.Value
                });
            }
            return response;
        }

        public Task<int> GetMoviesCount(string title = "")
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReviewResponseModel>> GetReviewsForMovie(int id)
        {
            var reviews = await _movieRepository.GetMovieReviews(id);
            var reviewDetails = new List<ReviewResponseModel>();
            foreach (var review in reviews)
            {
                reviewDetails.Add(new ReviewResponseModel
                {
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }
            return reviewDetails;
        }

        public async Task<IEnumerable<MovieRatingResponseModel>> GetTopRatedMovies()
        {
            var movies = await _movieRepository.GetTopRatedMovies();
            
            var movieResponse = new List<MovieRatingResponseModel>();
            foreach (var movie in movies)
            {
                movieResponse.Add(new MovieRatingResponseModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title,
                    ReleaseDate = movie.ReleaseDate,
                    Rating = movie.Rating
                });
            }
            return movieResponse;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetTopRevenueMovies()
        {
            // Repository?
            // MovieRepository class

            var movies = await _movieRepository.GetHighestRevenueMovies();
            // Map our Movie Entity to MovieResponseModel
            var movieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                //if(ReleaseDate!=null)
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    //ReleaseDate = movie.ReleaseDate.Value,
                    Title = movie.Title
                });
            }
            return movieResponseModel;
        }

        public async Task<MovieDetailsResponseModel> UpdateMovie(MovieCreateRequest movieCreateRequest)
        {
            //var dbMovie = await _movieRepository.GetMovieByTitle(movieCreateRequest.Title);
            //if (dbMovie == null)
            //{
              //  throw new Exception("Movie Not Found");
            //}
            var movie = new Movie
            {
                Title = movieCreateRequest.Title,
                Overview = movieCreateRequest.Overview,
                Tagline = movieCreateRequest.Tagline,
                Revenue = movieCreateRequest.Revenue,
                Budget = movieCreateRequest.Budget,
                ImdbUrl = movieCreateRequest.ImdbUrl,
                TmdbUrl = movieCreateRequest.TmdbUrl,
                PosterUrl = movieCreateRequest.PosterUrl,
                BackdropUrl = movieCreateRequest.BackdropUrl,
                OriginalLanguage = movieCreateRequest.OriginalLanguage,
                ReleaseDate = movieCreateRequest.ReleaseDate,
                RunTime = movieCreateRequest.RunTime,
                Price = movieCreateRequest.Price
            };
            var updateMovie = await _movieRepository.UpdateAsync(movie);
            var response = new MovieDetailsResponseModel
            {
                //Id = updateMovie.Id,
                Title = updateMovie.Title,
                Overview = updateMovie.Overview,
                PosterUrl=updateMovie.PosterUrl,
                BackdropUrl=updateMovie.BackdropUrl,
                Price=updateMovie.Price,
                Budget=updateMovie.Budget
            };
            return response;
        }
    }
}
