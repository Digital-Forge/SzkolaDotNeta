using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
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
        //////////////////////////////////////////////////////////////// Users
       
        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult Users()
        {
            ViewData["mode"] = "UserModerator";
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
                    return RedirectToAction("DetailsUser", new { id = buff });
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
            var buff = _usersModerateService.GetEditUser(id, HttpContext.User);
            if (buff != null) return View(buff);
            else return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult EditUser(DetailsEditUserVM input)
        {
            var buff = _usersModerateService.UpdateEditUser(input, HttpContext.User);
            if (buff) return RedirectToAction("DetailsUser", new { input.Id });
            else return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DeleteUser(string id)
        {
            switch (_usersModerateService.DeleteUser(id, HttpContext.User))
            {
                case -1:
                    return BadRequest();
                case 0:
                    return RedirectToAction("Users");
                case 1:
                    return Unauthorized();
                case 2:
                    return DeleteBlock(id);
                default:
                    return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "UserModerator, Admin")]
        public IActionResult DeleteBlock(string id)
        {
            return View(_usersModerateService.ReservationListByUser(id));
        }
    }
}