using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize(Roles = "UserModerator, ItemModerator, DepartmentModerator, Admin")]
    public class ModerateController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult Resources()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult Departments()
        {
            return View();
        }


    }
}