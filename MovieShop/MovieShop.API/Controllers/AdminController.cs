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
        [HttpPost]
        [Route("movie")]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest movieCreateRequestModel)
        {
            
            if (ModelState.IsValid)
            {
                var movie = await _movieService.CreateMovie(movieCreateRequestModel);
                return Ok(movieCreateRequestModel);
            }
            return BadRequest(new { message = "please correct the formate of the movie input information" });
        }
    }
}
