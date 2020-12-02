using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            if (ModelState.IsValid)
            {
                await _movieService.CreateMovie(movieCreateRequest);
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MovieByGenre(int genreId)
        {
            return View();
        }
        public IActionResult Details(int movieId)
        {
            return View();
        }
    }
}
