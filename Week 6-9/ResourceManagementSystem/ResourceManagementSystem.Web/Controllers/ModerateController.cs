using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceManagementSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementSystem.Web.Controllers
{
    [Authorize(Roles = "UserModerator, ItemModerator, DepartmentModerator, Admin")]
    public partial class ModerateController : Controller
    {
        private readonly IUsersModerateService _usersModerateService;
        private readonly IDepartmentModerateService _departmentModerateService;
        private readonly IItemModerateService _itemModerateService;

        public ModerateController(IUsersModerateService usersModerateService,
                                  IDepartmentModerateService departmentModerateService,
                                  IItemModerateService itemModerateService)
        {
            _usersModerateService = usersModerateService;
            _departmentModerateService = departmentModerateService;
            _itemModerateService = itemModerateService;
        }  
    }
}