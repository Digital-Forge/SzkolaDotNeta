using ResourceManagementSystem.Application.ViewModel.Users;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
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
        CreateUserVM CreateUser(ClaimsPrincipal moderator);
        string CreateUser(CreateUserVM input, ClaimsPrincipal user);
        bool SetActiveUser(bool status, AppUser user);
        DetailsEditUserVM UserDetails(string userId);
        DetailsEditUserVM GetEditUser(string userId, ClaimsPrincipal moderator);
        bool UpdateEditUser(DetailsEditUserVM input, ClaimsPrincipal moderatorS);
        short DeleteUser(string userId, ClaimsPrincipal moderator);
        List<AddRemoveStatusVM> ReservationListByUser(string userId);
    }
}
