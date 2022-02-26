using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize]
    public class InitController : Controller
    {
        readonly IInitService _initService;

        public InitController(IInitService initService)
        {
            _initService = initService;
        }

        [HttpGet]
        public IActionResult AdminAccount()
        {
            _initService.SetDefaultConfig();
            _initService.SetFirstAdminConfig(HttpContext.User);

            return View(new AdminAccountVM());
        }

        [HttpPost]
        public IActionResult AdminAccount(AdminAccountVM input)
        {
            if (_initService.SetFirstAdminData(input)) return RedirectToAction("Index", "Home"); ;
            return BadRequest();
        }
    }
}
