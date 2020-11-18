using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Details(int userId)
        {
            return View();
        }
    }
}
