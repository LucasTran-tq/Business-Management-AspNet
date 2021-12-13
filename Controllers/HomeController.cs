using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using App.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> SignInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            SignInManager = signInManager;
        }

        public string HiHome() => "Xin chao cac ban, toi la HiHome";
        public IActionResult Index()
        {
            if (SignInManager.IsSignedIn(User))
            {
                return Redirect("admin/salary-management/salary/ShowChart");
            }
            else
            {
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
