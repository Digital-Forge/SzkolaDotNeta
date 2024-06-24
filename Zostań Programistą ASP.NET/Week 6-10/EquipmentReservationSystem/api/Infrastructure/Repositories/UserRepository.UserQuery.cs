using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Domain.Interfaces.Repositories.IUserRepository;

namespace Infrastructure.Repositories
{
    public partial class UserRepository
    {
        private class UserQuery : IUserQuery
        {
            public UserQuery(Context _context, bool onlyActive, bool asNoTracking, bool allowBuffor)
            {
                AsNoTracking = asNoTracking;
                OnlyActive = onlyActive;
                AllowBuffer = allowBuffor;

                _query = _context.Users;
            }

            public bool AsNoTracking { get; }

            public bool OnlyActive { get; }

            public bool AllowBuffer { get; }

            private IQueryable<UserData> _query;
            public IQueryable<UserData> Query {
                get 
                {
                    if (AsNoTracking) _query = _query.AsNoTracking();
                    if (OnlyActive) _query = _query.Where(x => x.Active);

                    return _query.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer)));
                } 
            }

            public IUserQuery IncludeDepartments()
            {
                _query = _query
                    .Include(i => i.Departments.Where(x => !(x.Department.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.Department.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(i => i.Department);

                return this;
            }

            public IUserQuery IncludeDepartmentsWithItems()
            {
                _query = _query
                    .Include(i => i.Departments.Where(x => !(x.Department.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.Department.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(i => i.Department)
                    .ThenInclude(i => i.Items.Where(x => !(x.Item.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.Item.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(i => i.Item);

                return this;
            }

            public IUserQuery IncludeReservation()
            {
                _query = _query
                    .Include(i => i.Reservations.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))));

                return this;
            }

            public IUserQuery IncludeReservationItem()
            {
                _query = _query
                    .Include(i => i.Reservations.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(i => i.ItemInstance)
                    .ThenInclude(i => i.Item);

                return this;
            }

            public IUserQuery Where(System.Linq.Expressions.Expression<Func<UserData, bool>> predicate)
            {
                _query = _query.Where(predicate);
                return this;
            }

            public UserData? GetUserById(Guid id)
            {
                return Query.FirstOrDefault(x => x.Id == id);
            }

            public async Task<UserData?> GetUserByIdAsync(Guid id)
            {
                return await Query.FirstOrDefaultAsync(x => x.Id == id);
            }

            public UserData? GetUserByUsername(string username)
            {
                return Query.FirstOrDefault(x => x.NormalizedUserName == username.ToUpper());
            }

            public async Task<UserData?> GetUserByUsernameAsync(string username)
            {
                return await Query.FirstOrDefaultAsync(x => x.NormalizedUserName == username.ToUpper());
            }

            public UserData? GetUserByEmail(string email)
            {
                return Query.FirstOrDefault(x => x.NormalizedEmail == email.ToUpper());
            }

            public async Task<UserData?> GetUserByEmailAsync(string email)
            {
                return await Query.FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper());
            }

            public List<UserData> GetUsers()
            {
                return Query.ToList();
            }

            public async Task<List<UserData>> GetUsersAsync()
            {
                return await Query.ToListAsync();
            }
        }
    }
}
