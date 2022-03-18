using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Departments;
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
            return View(_departmentModerateService.GetDepartmentsList());
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult CreateDepartment()
        {
            return View(new CreateDepartmentVM());
        }

        [HttpPost]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult CreateDepartment(CreateDepartmentVM input)
        {
            switch (_departmentModerateService.CreateDepartment(input))
            {
                case -1:
                    return BadRequest();
                case 0:
                    return RedirectToAction("Departments");
                case 1:
                    ViewBag.Info = "Department with this name exist";
                    return View(input);
                default:
                    return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult DetailsDepartment(string id)
        {
            return View(_departmentModerateService.GetDetailsEdit(id));
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult EditDepartment(string id)
        {
            return View(_departmentModerateService.GetDetailsEdit(id));
        }

        [HttpPost]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult EditDepartment(DetailsEditDepartmentVM input)
        {
            if (_departmentModerateService.Update(input)) return RedirectToAction("DetailsDepartment", input.Id);
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "DepartmentModerator, Admin")]
        public IActionResult DeleteDepartment(string id)
        {
            if (_departmentModerateService.Delete(id)) return RedirectToAction("Departments");
            return BadRequest();
        }
    }
}