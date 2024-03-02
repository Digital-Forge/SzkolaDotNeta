using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Domain.Models.Business.MiddleTabs;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IDepartmentService))]
    public partial class DepartmentService(IDepartmentRepository _departmentRepository) : IDepartmentService
    {
        public async Task<ICollection<IDepartmentService.DepartmentBaseModel>> GetAllAsync()
        {
            return await _departmentRepository.GetAllFullQuery().Select(s => new IDepartmentService.DepartmentBaseModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                UserCount = s.Users.Count,
                ItemTypeCount = s.Items.Count,
            }).ToListAsync();
        }

        public async Task<Guid> AddAsync(IDepartmentService.DepartmentBaseModel model)
        {
            var entity = new Department()
            {
                Name = model.Name,
                Description = model.Description,
            };

            return await _departmentRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IDepartmentService.DepartmentFullModel> GetFullAsync(Guid id)
        {
            var dep = await _departmentRepository.GetFullAsync(id) ?? throw new NotFoundDepartment();

            return new IDepartmentService.DepartmentFullModel 
            { 
                Id = dep.Id,
                Name = dep.Name,
                Description= dep.Description,
                Create = DateOnly.FromDateTime(dep.CreateTime),
                Users = dep.Users.Select(s => new IDepartmentService.DepartmentUser
                {
                    Id = s.User.Id,
                    Fullname = s.User.UserName
                }).ToList(),
                Items = dep.Items.Select(s => new IDepartmentService.DepartmentItem
                {
                    Id= s.Item.Id,
                    Name = s.Item.Name,
                }).ToList(),
            };

        }

        public async Task<Guid> Create(IDepartmentService.DepartmentFullModel model)
        {
            var entity = new Department()
            {
                Name = model.Name,
                Description = model.Description,
                EntityStatus = Domain.Utils.EntityStatus.Buffer
            };

            await _departmentRepository.Save(entity);

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

            await _departmentRepository.Save(entity);
            return entity.Id;
        }

        public async Task Update(IDepartmentService.DepartmentFullModel model)
        {
            var entity = await _departmentRepository.GetFullAsync(model.Id.Value);

            entity.Name = model.Name;
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

            await _departmentRepository.Save(entity);
            return;
        }
    }
}
