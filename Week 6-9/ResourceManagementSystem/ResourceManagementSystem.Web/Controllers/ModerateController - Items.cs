using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize(Roles = "UserModerator, ItemModerator, DepartmentModerator, Admin")]
    public partial class ModerateController : Controller
    {
        //////////////////////////////////////////////////////////////// Items

        [HttpGet]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult Resources()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult CreateItem()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DetailsItem(string id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult EditItem(string id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DeleteItem(string id)
        {
            return View();
        }
    }
}