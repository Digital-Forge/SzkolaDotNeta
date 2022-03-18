using AutoMapper;
using AutoMapper.QueryableExtensions;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Departments;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using ResourceManagementSystem.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Application.Services
{
    public class DepartmentModerateService : IDepartmentModerateService
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public DepartmentModerateService(IDepartmentRepository departmentRepository,
                                         IUserRepository userRepository,
                                         IMapper mapper)
        {
            _departmentRepo = departmentRepository;
            _userRepo = userRepository;
            _mapper = mapper;
        }

        public List<DepartmentOfListVM> GetDepartmentsList()
        {
            return _departmentRepo.GetDepartmentsList().ProjectTo<DepartmentOfListVM>(_mapper.ConfigurationProvider).ToList();
        }

        public int CreateDepartment(CreateDepartmentVM input)
        {
            var buff = _departmentRepo.GetDepartmentsList().Where(x => x.Name.ToLower() == input.Name.ToLower()).Count();
            if (buff != 0) return 1;
            return string.IsNullOrEmpty(_departmentRepo.AddDepartment(input.Name)) ? -1 : 0;
        }

        public bool Delete(string id)
        {
            var department = _departmentRepo.GetDepartmentById(id);

            foreach (var user in department.AppUsers)
            {
                _departmentRepo.RemoveUserFromDepartment(user.AppUserId, department.Id);
            }

            foreach (var item in department.Items)
            {
                _departmentRepo.RemoveItemFromDepartment(item.ItemId, department.Id);
            }

            return _departmentRepo.DeleteDepartmentById(department.Id);
        }

        public DetailsEditDepartmentVM GetDetailsEdit(string id)
        {
            var department = _departmentRepo.GetDepartmentById(id);
            var VM = _mapper.Map<DetailsEditDepartmentVM>(department);

            VM.UsersList = department.AppUsers
                .Select(x => new AddRemoveStatusVM
                { 
                    Id = x.AppUserId,
                    Name = x.AppUser.FullName,
                    Status = true
                }).ToList();

            VM.ItemsList = department.Items
                .Select(x => new AddRemoveStatusVM
                {
                    Id = x.ItemId,
                    Name = x.Item.Name,
                    Status = true
                }).ToList();

            return VM;
        }

        public bool Update(DetailsEditDepartmentVM input)
        {
            foreach (var item in input.ItemsList)
            {
                if (!item.Status)
                {
                    _departmentRepo.RemoveItemFromDepartment(item.Id, input.Id);
                }
            }

            foreach (var item in input.UsersList)
            {
                if (!item.Status)
                {
                    _departmentRepo.RemoveItemFromDepartment(item.Id, input.Id);
                }
            }

            var buff = _departmentRepo.GetDepartmentById(input.Id);

            buff.Name = input.Name;
            return _departmentRepo.UpdateDepartment(buff);
        }
    }
}
