using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResourceManagementSystem.Web.Models;
using System.Diagnostics;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string panel)
        {
            switch (panel)
            {
                case "User":
                    return RedirectToAction("Index");
                    //break;
                case "UserModerator":
                    return RedirectToAction("Users", "Moderate");
                    //break;
                case "ItemModerator":
                    return RedirectToAction("Resources", "Moderate");
                    //break;
                case "DepartmentModerator":
                    return RedirectToAction("Departments", "Moderate");
                    //break;
                case "PickupPoint":
                    return RedirectToAction("Index", "PickupPoint");
                    //break;
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                    //break;
            }
            return BadRequest();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
