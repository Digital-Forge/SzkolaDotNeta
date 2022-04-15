using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Items;
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
            ViewData["mode"] = "ItemModerator";
            return View(_itemModerateService.GetItemsList());
        }

        [HttpGet]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult CreateItem()
        {
            return View(_itemModerateService.GetCreateOfItemTemplate());
        }

        [HttpPost]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult CreateItem(ItemVM input)
        {
            var buff = _itemModerateService.CreateItem(input);
            if (buff != "") return RedirectToAction("DetailsItem", new { input.Id });
            else return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult DetailsItem(string id)
        {
            return View(_itemModerateService.GetDetailsEditItem(id));
        }

        [HttpGet]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult EditItem(string id)
        {
            return View(_itemModerateService.GetDetailsEditItem(id));
        }

        [HttpPost]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult EditItem(ItemVM input)
        {
            if (_itemModerateService.UpdateItem(input)) return RedirectToAction("DetailsItem", new { input.Id });
            else return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "ItemModerator, Admin")]
        public IActionResult DeleteItem(string id)
        {
            if (_itemModerateService.DeleteItem(id)) return RedirectToAction("Resources");
            else return BadRequest();
        }
    }
}