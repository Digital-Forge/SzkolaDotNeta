using AutoMapper;
using AutoMapper.QueryableExtensions;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Departments;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
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
            var department = _departmentRepo.GetDepartmentsList().Where(x => x.Id.ToString().ToLower() == id);

            foreach (var user in department.SelectMany(x => x.AppUsers))
            {
                _departmentRepo.RemoveUserFromDepartment(user.AppUserId, id);
            }

            foreach (var item in department.SelectMany(x => x.Items))
            {
                _departmentRepo.RemoveItemFromDepartment(item.ItemId, new Guid(id));
            }

            return _departmentRepo.DeleteDepartmentById(id);
        }

        public DetailsEditDepartmentVM GetDetailsEdit(string id)
        {
            var department = _departmentRepo.GetDepartmentsList().Where(x => x.Id.ToString().ToLower() == id);
            var VM = _mapper.Map<DetailsEditDepartmentVM>(department.ToList()[0]);

            VM.UsersList = department?.SelectMany(x => x.AppUsers)
                ?.Select(x => new AddRemoveStatusVM
                { 
                    Id = x.AppUserId,
                    Name = x.AppUser.FullName,
                    Status = true
                }).ToList() ?? new List<AddRemoveStatusVM>();

            VM.ItemsList = department?.SelectMany(x => x.Items)
                ?.Select(x => new AddRemoveStatusVM
                {
                    Id = x.ItemId.ToString().ToLower(),
                    Name = x.Item.Name,
                    Status = true
                }).ToList() ?? new List<AddRemoveStatusVM>();

            return VM;
        }

        public bool Update(DetailsEditDepartmentVM input)
        {
            foreach (var item in input.ItemsList ?? Enumerable.Empty<AddRemoveStatusVM>())
            {
                if (!item.Status)
                {
                    _departmentRepo.RemoveItemFromDepartment(item.Id, input.Id);
                }
            }

            foreach (var user in input.UsersList ?? Enumerable.Empty<AddRemoveStatusVM>())
            {
                if (!user.Status)
                {
                    _departmentRepo.RemoveUserFromDepartment(user.Id, input.Id);
                }
            }

            var buff = _departmentRepo.GetDepartmentById(input.Id);

            buff.Name = input.Name;
            return _departmentRepo.UpdateDepartment(buff);
        }

        public StatusUsersInDepartmentVM GetListToAddUsers(string departmentId)
        {
            var buff = _departmentRepo.GetDepartmentById(departmentId);

            var VM = new StatusUsersInDepartmentVM
            {
                Id = buff.Id.ToString().ToLower(),
                Name = buff.Name,
                UsersList = new List<AddRemoveStatusVM>()
            };

            VM.UsersList = _userRepo.GetUsersList().Select(x => new AddRemoveStatusVM
            {
                Id = x.Id,
                Name = x.FullName,
                Status = x.Departments != null ? x.Departments.Any(x => x.DepartmentId.ToString().ToLower() == departmentId) : false
            }).ToList();

            return VM;
        }

        public void UpdateUsersInDepartment(StatusUsersInDepartmentVM input)
        {
            var department = _departmentRepo.GetDepartmentsList().Where(x => x.Id.ToString().ToLower() == input.Id).SelectMany(x => x.AppUsers);

            foreach (var user in input.UsersList ?? Enumerable.Empty<AddRemoveStatusVM>())
            {
                if (user.Status)
                {
                    if (!(department?.Any(x => x.AppUserId == user.Id) ?? false))
                    {
                        _departmentRepo.AddUserToDepartment(user.Id, input.Id);
                    }
                }
                else
                {
                    if (department?.Any(x => x.AppUserId == user.Id) ?? false)
                    {
                        _departmentRepo.RemoveUserFromDepartment(user.Id, input.Id);
                    }
                }
            }
        }
    }
}
