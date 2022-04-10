using AutoMapper;
using AutoMapper.QueryableExtensions;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using ResourceManagementSystem.Application.ViewModel.Items;
using ResourceManagementSystem.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Application.Services
{
    public class ItemModerateService : IItemModerateService
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepo;
        private readonly IDepartmentRepository _departmentRepo;

        public ItemModerateService(IItemRepository itemRepository,
                                   IMapper mapper,
                                   IDepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _itemRepo = itemRepository;
            _departmentRepo = departmentRepository;
        }

        public List<ItemOfListVM> GetItemsList()
        {
            return _itemRepo.GetItemList().ProjectTo<ItemOfListVM>(_mapper.ConfigurationProvider).ToList();
        }

        public ItemVM GetCreateOfItemTemplate()
        {
            return new ItemVM()
            {
                DepartmentsList = _departmentRepo.GetDepartmentsList()
                .Select(x => new AddRemoveStatusVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = false
                }).ToList()
            };
        }

        public ItemVM GetDetailsEditItem(string id)
        {
            throw new NotImplementedException();
        }

        public int CreateItem(ItemVM input)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(ItemVM input)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(string id)
        {
            throw new NotImplementedException();
        }
    }
}
