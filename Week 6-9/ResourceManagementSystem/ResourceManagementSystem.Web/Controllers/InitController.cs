using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize]
    public class InitController : Controller
    {
        [HttpGet]
        public IActionResult AdminAccount()
        {
            return View();
        }
    }
}
