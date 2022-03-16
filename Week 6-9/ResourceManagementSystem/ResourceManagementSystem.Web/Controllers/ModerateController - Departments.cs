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
        //////////////////////////////////////////////////////////////// Departments

        [HttpGet]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult Departments()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DetailsDepartment(string id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult EditDepartment(string id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DeleteDepartment(string id)
        {
            return View();
        }
    }
}