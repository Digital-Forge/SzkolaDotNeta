using ResourceManagementSystem.Application.ViewModel.Users;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ResourceManagementSystem.Application.Interfaces
{
    public interface IUsersModerateService
    {
        List<UserOfListVM> ListOfUsers();
        CreateUserVM CreateUser(ClaimsPrincipal user);
        string CreateUser(CreateUserVM input, ClaimsPrincipal user);
        bool SetActiveUser(bool status, AppUser user);
        DetailsEditUserVM UserDetails(string userId);
    }
}
