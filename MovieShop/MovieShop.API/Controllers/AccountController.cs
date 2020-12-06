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
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserDetails(id);
            if (user == null)
            {
                return NotFound("No User Found");
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(UserRegisterRequestModel userRegisterRequestModel)
        {
            if (ModelState.IsValid)
            {
                //call the user service
                await _userService.CreateUser(userRegisterRequestModel);
                return Ok(userRegisterRequestModel);
            }
            return BadRequest(new { message = "please correct the input information" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestModel loginRequestModel)
        {
            if (ModelState.IsValid)
            {
                var userLogin = await _userService.ValidateUser(loginRequestModel.Email, loginRequestModel.Password);
                if (userLogin != null)
                {
                    return Ok(userLogin);
                }
                return BadRequest(new { message = "Invalid email or password" });
            }
            return BadRequest(new { message = "Invalid email or password" });
        }
    }
}
