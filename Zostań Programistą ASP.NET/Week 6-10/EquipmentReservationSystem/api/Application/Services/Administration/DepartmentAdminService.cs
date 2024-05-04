using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Domain.Models.Business.MiddleTabs;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IDepartmentAdminService))]
    public partial class DepartmentAdminService(IDepartmentRepository _departmentRepository) : IDepartmentAdminService
    {
        public async Task<ICollection<IDepartmentAdminService.DepartmentTableModel>> GetAllAsync()
        {
            return await _departmentRepository.QueryBuilder(false, true, false)
                .IncludeItems()
                .IncludeUsers()
                .Query
                .OrderBy(o => o.Name)
                .Select(s => new IDepartmentAdminService.DepartmentTableModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Active = s.Active,
                    Description = s.Description,
                    UserCount = s.Users.Count,
                    ItemTypeCount = s.Items.Count,
                }).ToListAsync();
        }

        public async Task<ICollection<IDepartmentAdminService.DepartmentComboModel>> GetAllComboAsync()
        {
            return await _departmentRepository.QueryBuilder(true, true, false)
                .Query
                .OrderBy(o => o.Name)
                .Select(s => new IDepartmentAdminService.DepartmentComboModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IDepartmentAdminService.DepartmentModel> GetModelAsync(Guid id)
        {
            var dep = await _departmentRepository.QueryBuilder(asNoTracking: true)
                .IncludeItems()
                .IncludeUsers()
                .GetDepartmentByIdAsync(id) ?? throw new NotFoundDepartment();

            return new IDepartmentAdminService.DepartmentModel
            {
                Id = dep.Id,
                Name = dep.Name,
                Active = dep.Active,
                Description = dep.Description,
                Create = DateOnly.FromDateTime(dep.CreateTime),
                Users = dep.Users.Select(s => new IDepartmentAdminService.DepartmentUserModel
                {
                    Id = s.User.Id,
                    Fullname = s.User.UserName,
                    Email = s.User.Email,                   
                }).ToList(),
                Items = dep.Items.Select(s => new IDepartmentAdminService.DepartmentItemModel
                {
                    Id = s.Item.Id,
                    Name = s.Item.Name,
                }).ToList(),
            };
        }

        public async Task<Guid> CreateAsync(IDepartmentAdminService.DepartmentModel model)
        {
            var entity = new Department()
            {
                Name = model.Name,
                Description = model.Description,
                Active = model.Active,
                EntityStatus = Domain.Utils.EntityStatus.Buffer,
            };

            await _departmentRepository.SaveAsync(entity);

            entity.Users = model.Users.Select(s => new UserToDepartment()
            {
                DepartmentId = entity.Id,
                UserId = s.Id
            }).ToList();

            entity.Items = model.Items.Select(s => new ItemToDepartment()
            {
                DepartmentId = entity.Id,
                ItemId = s.Id
            }).ToList();

            entity.EntityStatus = Domain.Utils.EntityStatus.Use;

            await _departmentRepository.SaveAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(IDepartmentAdminService.DepartmentModel model)
        {
            var entity = await _departmentRepository.QueryBuilder()
                .IncludeUsers()
                .IncludeItems()
                .GetDepartmentByIdAsync(model.Id.Value);

            entity.Name = model.Name;
            entity.Active = model.Active;
            entity.Description = model.Description;

            entity.Users = model.Users.Select(s => new UserToDepartment()
            {
                DepartmentId = entity.Id,
                UserId = s.Id
            }).ToList();

            entity.Items = model.Items.Select(s => new ItemToDepartment()
            {
                DepartmentId = entity.Id,
                ItemId = s.Id
            }).ToList();

            await _departmentRepository.SaveAsync(entity);
            return;
        }

        public IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableData GetTable(IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableOptions options)
        {
            var data = new IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableData();

            var query = _departmentRepository.QueryBuilder(false, true, false)
                .IncludeItems()
                .IncludeUsers()
                .Query;

            if (!string.IsNullOrWhiteSpace(options.Search)) query = query.Where(x => x.Name.Contains(options.Search));

            data.TotalCount = query.Count();
            query = query.OrderBy(o => o.Name);
            query = query.Skip(options.Skip ?? 0);
            query = query.Take(options.Take ?? 0);

            data.Data = query.Select(s => new IDepartmentAdminService.DepartmentTableModel
            {
                Id = s.Id,
                Name = s.Name,
                Active = s.Active,
                Description = s.Description,
                UserCount = s.Users.Count,
                ItemTypeCount = s.Items.Count,
            }).ToList();

            return data;
        }

        public async Task<IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableData> GetTablAsync(IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableOptions options)
        {
            var data = new IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableData();

            var query = _departmentRepository.QueryBuilder(false, true, false)
                .IncludeItems()
                .IncludeUsers()
                .Query;

            if (!string.IsNullOrWhiteSpace(options.Search)) query = query.Where(x => x.Name.Contains(options.Search));

            data.TotalCount = await query.CountAsync();
            query = query.OrderBy(o => o.Name);
            query = query.Skip(options.Skip ?? 0);
            query = query.Take(options.Take ?? 0);

            data.Data = await query.Select(s => new IDepartmentAdminService.DepartmentTableModel
            {
                Id = s.Id,
                Name = s.Name,
                Active = s.Active,
                Description = s.Description,
                UserCount = s.Users.Count,
                ItemTypeCount = s.Items.Count,
            }).ToListAsync();

            return data;
        }
    }
}
