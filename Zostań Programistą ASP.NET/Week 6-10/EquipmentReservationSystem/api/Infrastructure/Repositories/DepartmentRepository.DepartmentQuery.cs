using Domain.Models.Business;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Domain.Interfaces.Repositories.IDepartmentRepository;

namespace Infrastructure.Repositories
{
    public partial class DepartmentRepository
    {
        private class DepartmentQuery : IDepartmentQuery
        {
            public DepartmentQuery(Context context, bool onlyActive, bool asNoTracking, bool allowBuffer)
            {
                OnlyActive = onlyActive;
                AsNoTracking = asNoTracking;
                AllowBuffer = allowBuffer;

                _query = context.Departments;
            }

            public bool AsNoTracking { get; }

            public bool OnlyActive { get; }
            
            public bool AllowBuffer { get; }
            
            private IQueryable<Department> _query;
            public IQueryable<Department> Query
            {
                get
                {
                    if (AsNoTracking) _query = _query.AsNoTracking();
                    if (OnlyActive) _query = _query.Where(x => x.Active);

                    return _query.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer)));
                }
            }

            public IDepartmentQuery IncludeItems()
            {
                _query = _query
                    .Include(i => i.Items.Where(x => !(x.Item.EntityStatus == Domain.Utils.EntityStatus.Delete || (AllowBuffer && x.Item.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(i => i.Item);

                return this;
            }

            public IDepartmentQuery IncludeUsers()
            {
                _query = _query
                    .Include(i => i.Users.Where(x => !(x.User.EntityStatus == Domain.Utils.EntityStatus.Delete || (AllowBuffer && x.User.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(i => i.User);

                return this;
            }

            public IDepartmentQuery Where(Expression<Func<Department, bool>> predicate)
            {
                _query = _query.Where(predicate);
                return this;
            }

            public Department? GetDepartmentById(Guid id)
            {
                return Query.FirstOrDefault(x => x.Id == id);
            }

            public async Task<Department?> GetDepartmentByIdAsync(Guid id)
            {
                return await Query.FirstOrDefaultAsync(x => x.Id == id);
            }

            public Department? GetDepartmentByName(string name)
            {
                return Query.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            }

            public async Task<Department?> GetDepartmentByNameAsync(string name)
            {
                return await Query.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            }

            public List<Department> GetDepartments()
            {
                return Query.ToList();
            }

            public async Task<List<Department>> GetDepartmentsAsync()
            {
                return await Query.ToListAsync();
            }
        }
    }
}
