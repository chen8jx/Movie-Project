using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //[Authorize]
        [HttpPost]
        [Route("purchase")]
        public async Task<ActionResult> CreatePurchase(PurchaseRequestModel purchaseRequest)
        {
            if (ModelState.IsValid)
            {
                await _userService.PurchaseMovie(purchaseRequest);
                return Ok();
            }
            return BadRequest(new { message = "please correct the input information" });
        }
        //[Authorize]
        [HttpPost]
        [Route("favorite")]
        public async Task<ActionResult> FavoritePurchase(FavoriteRequestModel favoriteRequest)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddFavorite(favoriteRequest);
                return Ok();
            }
            return BadRequest(new { message = "please correct the input information" });
        }
        //[Authorize]
        [HttpPost]
        [Route("unfavorite")]
        public async Task<ActionResult> UnfavoritePurchase(FavoriteRequestModel favoriteRequest)
        {
            if (ModelState.IsValid)
            {
                await _userService.RemoveFavorite(favoriteRequest);
                return Ok();
            }
            return BadRequest(new { message = "please correct the input information" });
        }
        //[Authorize]
        [HttpGet]
        [Route("{id:int}/movie/{movieId}/favorite")]
        public async Task<ActionResult> IsFavoriteExists(int id, int movieId)
        {
            var favoriteExists = await _userService.FavoriteExists(id, movieId);
            return Ok(new { isFavorited = favoriteExists });
        }
        //[Authorize]
        [HttpPost]
        [Route("review")]
        public async Task<ActionResult> CreateReview(ReviewRequestModel reviewRequest)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddMovieReview(reviewRequest);
                return Ok();
            }
            return BadRequest(new { message = "please correct the input information" });
        }
        //[Authorize]
        [HttpPut("review")]
        public async Task<ActionResult> UpdateReview(ReviewRequestModel reviewRequest)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateMovieReview(reviewRequest);
                return Ok();
            }
            return BadRequest(new { message = "please correct the input information" });
        }
        //[Authorize]
        [HttpDelete]
        [Route("{userId:int}/movie/{movieId:int}")]
        public async Task<ActionResult> DeleteReview(int userId, int movieId)
        {
            if (ModelState.IsValid)
            {
                await _userService.DeleteMovieReview(userId, movieId);
                return Ok();
            }
            return BadRequest("Please check the info you entered");
        }
        //[Authorize]
        [HttpGet]
        [Route("{id:int}/purchases")]
        public async Task<ActionResult> GetUserPurchasedMovies(int id)
        {
            var userMovies = await _userService.GetAllPurchasesForUser(id);
            if (userMovies == null)
            {
                return BadRequest(new { message = "No Purchased Movies" });
            }
            return Ok(userMovies);
        }
        //[Authorize]
        [HttpGet]
        [Route("{id:int}/favorites")]
        public async Task<ActionResult> GetUserFavoriteMovies(int id)
        {
            var userMovies = await _userService.GetAllFavoritesForUser(id);
            if (userMovies == null)
            {
                return BadRequest(new { message = "No Favorite Movies" });
            }
            return Ok(userMovies);
        }
        //[Authorize]
        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<ActionResult> GetUserReviewedMovies(int id)
        {
            var userMovies = await _userService.GetAllReviewsByUser(id);
            if (userMovies == null)
            {
                return BadRequest(new { message = "No Movie Reviews" });
            }
            return Ok(userMovies);
        }
    }
}
