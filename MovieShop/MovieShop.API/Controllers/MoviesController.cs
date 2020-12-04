using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Controllers
{//Attribute based Routing
    [Route("api/[controller]")]
    [ApiController]
    //ControllerBase(API) can be implemented by different controllers
    //Controller contains some things, like View, that are not required in API
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
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
    }
}