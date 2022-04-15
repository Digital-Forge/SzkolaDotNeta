using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize(Roles = "PickupPoint, Admin")]
    public class PickupPointController : Controller
    {
        public IActionResult Index()
        {
            ViewData["mode"] = "PickupPoint";
            return View();
        }
    }
}
