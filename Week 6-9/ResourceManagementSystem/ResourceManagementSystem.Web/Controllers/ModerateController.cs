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
    public class ModerateController : Controller
    {
        private readonly IUsersModerateService _usersModerateService;

        public ModerateController(IUsersModerateService usersModerateService)
        {
            _usersModerateService = usersModerateService;
        }

        //////////////////////////////////////////////////////////////// Users
       
        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult Users()
        {
            return View(_usersModerateService.ListOfUsers());
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult CreateUser()
        {
            return View(_usersModerateService.CreateUser(HttpContext.User));
        }

        [HttpPost]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult CreateUser(CreateUserVM input)
        {
            var buff = _usersModerateService.CreateUser(input, HttpContext.User);

            switch (buff)
            {
                case "1":
                    ViewBag.UsernameInfo = "User with that username exist";
                    return View();
                case "-1":
                    ViewBag.ErrorInfo = "Unidentified error";
                    return View();
                default:
                    return RedirectToAction("DetailsUser", buff);
            }
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DetailsUser(string id)
        {
            var buff = _usersModerateService.UserDetails(id);
            if (buff != null) return View(buff);
            else return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult EditUser(string id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult EditUser(DetailsEditUserVM input)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DeleteUser(string id)
        {
            return View();
        }

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