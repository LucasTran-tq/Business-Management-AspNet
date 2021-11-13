using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Blog;
using Microsoft.AspNetCore.Authorization;
using App.Data;
using Microsoft.Extensions.Logging;

namespace AppMvc.Net.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("admin/employee/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator+  "," + RoleName.Editor)]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        //public string HiHome() => "Xin chao cac ban, toi la HiHome";
        public IActionResult Index()
        {
            return View();
        }
    }
}
