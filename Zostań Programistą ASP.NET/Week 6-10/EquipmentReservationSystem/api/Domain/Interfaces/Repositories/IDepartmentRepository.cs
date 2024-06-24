using Domain.Interfaces.Repositories.Abstract;
using Domain.Models.Business;
using static Domain.Interfaces.Repositories.IDepartmentRepository;

namespace Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository : IRepository<IDepartmentQuery, Department>
    {
        interface IDepartmentQuery : IRepositoryQueryBuilder<Department>
        {
            IDepartmentQuery IncludeItems();
            IDepartmentQuery IncludeUsers();
            IDepartmentQuery Where(System.Linq.Expressions.Expression<Func<Department, bool>> predicate);
            Department? GetDepartmentById(Guid id);
            Task<Department?> GetDepartmentByIdAsync(Guid id);
            Department? GetDepartmentByName(string name);
            Task<Department?> GetDepartmentByNameAsync(string name);
            List<Department> GetDepartments();
            Task<List<Department>> GetDepartmentsAsync();
        }
    }
}
