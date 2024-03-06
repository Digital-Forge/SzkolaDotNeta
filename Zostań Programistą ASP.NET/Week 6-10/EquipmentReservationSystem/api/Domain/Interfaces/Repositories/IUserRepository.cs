using Domain.Interfaces.Repositories.Abstract;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using static Domain.Interfaces.Repositories.IUserRepository;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<IUserQuery, UserData>
    {
        UserData GetContextUser();
        UserData GetUser(string email);
        UserData GetUser(Guid id);
        IEnumerable<IdentityRole<Guid>> GetUserRoles(Guid userId);

        interface IUserQuery : IRepositoryQuerybuilder<UserData>
        {
            IUserQuery IncludeDepartments();
            IUserQuery IncludeReservation();
            IUserQuery Where(System.Linq.Expressions.Expression<Func<UserData, bool>> predicate);
            UserData? GetUserById(Guid id);
            Task<UserData?> GetUserByIdAsync(Guid id);
            UserData? GetUserByUsername(string username);
            Task<UserData?> GetUserByUsernameAsync(string username);
            UserData? GetUserByEmail(string email);
            Task<UserData?> GetUserByEmailAsync(string email);
            List<UserData> GetUsers();
            Task<List<UserData>> GetUsersAsync();
        }
    }
}
