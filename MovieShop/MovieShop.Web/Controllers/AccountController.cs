using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        //we need to show empty view
        //http:localhost/account/register GET
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        //when click submit, we post information to this method
        //http:localhost/account/register POST
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel userRegisterRequestModel)
        {
            //only wehn each and every validation in our is true we need to proceed further
            if (ModelState.IsValid)
            {
                //we need to sen the userRModel to our service
                await _userService.CreateUser(userRegisterRequestModel);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest,string returnUrl=null)
        {
            returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) return View();
            var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            //only when username/pass success code below will execute
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname,  user.LastName),
                new Claim(ClaimTypes.NameIdentifier,  user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Authorize]//check cookie if user is logged in
        public async Task<IActionResult> MyAccount()
        {
            return View();
        }
    }
}
