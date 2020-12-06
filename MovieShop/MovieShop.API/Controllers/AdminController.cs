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
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpPost]
        [Route("movie")]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest movieCreateRequest)
        {

            if (ModelState.IsValid)
            {
                var movie = await _movieService.CreateMovie(movieCreateRequest);
                return Ok(movieCreateRequest);
            }
            return BadRequest(new { message = "please correct the formate of the movie input information" });
        }
        [HttpPut("movie")]
        public async Task<IActionResult> UpdateMovie(MovieCreateRequest movieCreateRequest)
        {
            var movie = await _movieService.UpdateMovie(movieCreateRequest);
            return Ok(movie);
            //if (ModelState.IsValid)
            //{
            //    var movie = await _movieService.UpdateMovie(movieCreateRequest);
            //    return Ok(movie);
            //}
            //return BadRequest(new { message = "please correct the movie input information" });
        }
        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> PurchasedMovies(int pageSize = 20, int page = 1)
        {
            var movies = await _movieService.GetAllMoviePurchasesByPagination(pageSize, page);
            if (movies == null)
            {
                return BadRequest(new { message = "No Purchased Movies" });
            }
            return Ok(movies);
        }
        [HttpGet]
        [Route("top")]
        public async Task<IActionResult> TopPurchasedMovies()
        {
            return Ok();
        }
    }
}
