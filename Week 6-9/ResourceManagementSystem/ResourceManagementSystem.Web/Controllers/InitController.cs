using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Init;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize]
    public class InitController : Controller
    {
        private readonly IInitService _initService;
        private readonly SignInManager<AppUser> _signInManager;

        public InitController(IInitService initService,
                              SignInManager<AppUser> signInManager)
        {
            _initService = initService;
            _signInManager = signInManager;
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
            if (_initService.SetFirstAdminData(input))
            {
                _signInManager.SignOutAsync().Wait();
                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }
    }
}
