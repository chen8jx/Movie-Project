using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.API.Controllers
{//Attribute based Routing
    [Route("api/[controller]")]
    [ApiController]
    //ControllerBase(API) can be implemented by different controllers
    //Controller contains some things, like View, that are not required in API
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        // api/movies/toprevenue
        [HttpGet]
        [Route("toprevenue")] //System automatically create api/movies because of the controller name
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            //call our service and call the method, then get the data 
            //var movies = _movieService.GetTopMovies();

            //http status code
            var movies = await _movieService.GetTopRevenueMovies();
            if (!movies.Any())
            {
                return NotFound("No Movies Found");
            }
            return Ok(movies);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieAsync(id);
            if (movie == null)
            {
                return Ok(movie);
            }
            return NotFound("No Movies Found");
        }
        [HttpGet]
        [Route("toprated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMovies();
            if (!movies.Any())
            {
                return Ok(movies);
            }
            return NotFound("No Movies Found");
        }
        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId);
            if (movies != null)
            {
                return Ok(movies);
            }
            return NotFound("No Movies Found");
        }
        [HttpGet]
        [Route("{id}/reviews")]
        public async Task<IActionResult> GetMovieReviews(int id)
        {
            var reviews = await _movieService.GetReviewsForMovie(id);
            return Ok(reviews);
        }
    }
}