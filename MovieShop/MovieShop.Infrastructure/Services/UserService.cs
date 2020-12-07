using MovieShop.Core.Entities;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _encryptionService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        private readonly IAsyncRepository<Review> _reviewRepository;

        public UserService(IUserRepository userRepository, 
            ICryptoService encryptionService, 
            IPurchaseRepository purchaseRepository, 
            IAsyncRepository<Favorite> favoriteRepository,
            IAsyncRepository<Review> reviewRepository)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _purchaseRepository = purchaseRepository;
            _favoriteRepository = favoriteRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId))
                throw new Exception("Movie already Favorited");
            var favorite = new Favorite
            {
                UserId = favoriteRequest.UserId,
                MovieId = favoriteRequest.MovieId
            };
            await _favoriteRepository.AddAsync(favorite);
        }

        public async Task AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var movieReview = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };
            await _reviewRepository.AddAsync(movieReview);
        }

        public async Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel requestModel)
        {
            //make sure email not in db
            //we need to send email to our User Repo and see if the data exists for the email
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);

            if (dbUser != null && string.Equals(dbUser.Email, requestModel.Email, StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Email Already Exits");

            //first step is to create a random salt
            var salt = _encryptionService.CreateSalt();
            var hashedPassword = _encryptionService.HashPassword(requestModel.Password, salt);
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };
            var createdUser = await _userRepository.AddAsync(user);
            var response = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };
            return response;
        }

        public async Task DeleteMovieReview(int userId, int movieId)
        {
            var review = await _reviewRepository.ListAsync(r => r.UserId == userId && r.MovieId == movieId);
            await _reviewRepository.DeleteAsync(review.First());
        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            return await _favoriteRepository.GetExistsAsync(f => f.MovieId == movieId &&
                                                                 f.UserId == id);
        }

        public async Task<IEnumerable<FavoriteResponseModel>> GetAllFavoritesForUser(int id)
        {
            var favorites = await _favoriteRepository.ListAsync(f => f.UserId == id);
            var response = new List<FavoriteResponseModel>();
            foreach (var movie in favorites)
            {
                response.Add(new FavoriteResponseModel
                {
                    UserId = movie.UserId,
                    MovieId = movie.MovieId,
                    Id = movie.Id
                });
            }
            return response;
        }

        public async Task<IEnumerable<PurchaseResponseModel>> GetAllPurchasesForUser(int id)
        {
            var purchasedMovies = await _purchaseRepository.ListAsync(p => p.UserId == id);
            var response = new List<PurchaseResponseModel>();
            foreach(var movie in purchasedMovies)
            {
                response.Add(new PurchaseResponseModel
                {
                    UserId = movie.UserId,
                    MovieId = movie.MovieId,
                    PurchaseDateTime = movie.PurchaseDateTime
                });
            }
            return response;
        }

        public async Task<IEnumerable<ReviewResponseModel>> GetAllReviewsByUser(int id)
        {
            var reviews = await _reviewRepository.ListAsync(r => r.UserId == id);
            var response = new List<ReviewResponseModel>();
            foreach(var movie in reviews)
            {
                response.Add(new ReviewResponseModel
                {
                    UserId = movie.UserId,
                    MovieId = movie.MovieId,
                    Rating = movie.Rating,
                    ReviewText = movie.ReviewText
                });
            }
            return response;
        }

        //public Task<PagedResultSet<User>> GetAllUsersByPagination(int pageSize = 20, int page = 0, string lastName = "")
        //{
        //    throw new NotImplementedException();
        //}

        public Task<User> GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRegisterResponseModel> GetUserDetails(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            var response = new UserRegisterResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return response;
        }

        public Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest)
        {
            throw new NotImplementedException();
        }

        public async Task PurchaseMovie(PurchaseRequestModel purchaseRequest)
        {
            var movie = await _purchaseRepository.GetByIdAsync(purchaseRequest.MovieId);
            //var user = await _purchaseRepository.GetByIdAsync(purchaseRequest.UserId);
            purchaseRequest.TotalPrice = movie.TotalPrice;
            purchaseRequest.PurchaseNumber = movie.PurchaseNumber;
            purchaseRequest.PurchaseDateTime = movie.PurchaseDateTime;
            var purchase = new Purchase
            {
                UserId = purchaseRequest.UserId,
                MovieId = purchaseRequest.MovieId,
                TotalPrice = purchaseRequest.TotalPrice,
                PurchaseNumber = purchaseRequest.PurchaseNumber,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime
            };
            await _purchaseRepository.AddAsync(purchase);
        }

        public async Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var dbFavorite =
                await _favoriteRepository.ListAsync(r => r.UserId == favoriteRequest.UserId &&
                                                         r.MovieId == favoriteRequest.MovieId);
            await _favoriteRepository.DeleteAsync(dbFavorite.First());
        }

        public async Task UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var dbReview = await _reviewRepository.ListAsync(r => r.UserId == reviewRequest.UserId && r.MovieId == reviewRequest.MovieId);
            if (dbReview == null)
            {
                throw new Exception("No Review Found");
            }
            var review = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };
            await _reviewRepository.UpdateAsync(review);
        }

        public async Task<UserLoginResponseModel> ValidateUser(string email, string password)
        {
            //we check if the email exists in the db
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            var hashedPassword = _encryptionService.HashPassword(password, user.Salt);
            var isSuccess = user.HashedPassword == hashedPassword;

            var response = new UserLoginResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth
            };
            //var response = _mapper.Map<UserLoginResponseModel>(user);
            //var userRoles = roles.ToList();
            //if (userRoles.Any())
            //{
            //    response.Roles = userRoles.Select(r => r.Role.Name).ToList();
            //}
            if (isSuccess)
                return response;
            else
                return null;
        }
    }
}
