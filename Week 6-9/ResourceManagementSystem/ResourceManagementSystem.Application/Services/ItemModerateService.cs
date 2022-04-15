using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using ResourceManagementSystem.Application.ViewModel.Items;
using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Application.Services
{
    public class ItemModerateService : IItemModerateService
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IAppSettingPropertyRepository _appSettingPropertyRepo;
        private readonly IReservationRepository _reservationRepo;

        public ItemModerateService(IItemRepository itemRepository,
                                   IMapper mapper,
                                   IDepartmentRepository departmentRepository,
                                   IAppSettingPropertyRepository appSettingPropertyRepository,
                                   IReservationRepository reservationRepository)
        {
            _mapper = mapper;
            _itemRepo = itemRepository;
            _departmentRepo = departmentRepository;
            _appSettingPropertyRepo = appSettingPropertyRepository;
            _reservationRepo = reservationRepository;
        }

        public List<ItemOfListVM> GetItemsList()
        {
            return _itemRepo.GetItemList().ProjectTo<ItemOfListVM>(_mapper.ConfigurationProvider).ToList();
        }

        public ItemVM GetCreateOfItemTemplate()
        {
            return new ItemVM()
            {
                ImageObj = null,
                DepartmentsList = _departmentRepo.GetDepartmentsList()
                .Select(x => new AddRemoveStatusVM
                {
                    Id = x.Id.ToString().ToLower(),
                    Name = x.Name,
                    Status = false
                }).ToList()/*,
                SerialsList = new List<AddRemoveStatusVM>
                {
                    new AddRemoveStatusVM
                    {
                        Id = "0",
                        Name = "name",
                        Status = true
                    }
                }*/
            };
        }

        public ItemVM GetDetailsEditItem(string id)
        {
            var VM = _mapper.Map<ItemVM>(_itemRepo.GetItem(id));
            VM.ImageObj = null;

            var containDepartments = _itemRepo.GetDepartmentListByItem(id);

            VM.SerialsList = _itemRepo.GetSerialItemsListByItem(id).Select(x => new AddRemoveStatusVM
            {
                Id = x.Id.ToString().ToLower(),
                Name = $"{x.SerialNumber} - Reservated",
                Status = true
            }).ToList();

            VM.DepartmentsList = _departmentRepo.GetDepartmentsList().Select(x => new AddRemoveStatusVM
            {
                Id = x.Id.ToString().ToLower(),
                Name = x.Name,
                Status = containDepartments.Contains(x)
            }).ToList();

            return VM;
        }

        public string CreateItem(ItemVM input)
        {
            var item = _mapper.Map<Item>(input);
            item.ImagePath = UploadedImageSave(input.ImageObj);

            var itemId = _itemRepo.CreateItem(item);

            if (string.IsNullOrEmpty(itemId))
            {
                DeleteOldItemImage(item.ImagePath);
                return "";
            }

            foreach (var serial in input.SerialsList ?? Enumerable.Empty<AddRemoveStatusVM>())
            {
                _itemRepo.AddSerialToItem(new SerialItem { SerialNumber = serial.Name }, itemId);
            }

            foreach (var department in input.DepartmentsList ?? Enumerable.Empty<AddRemoveStatusVM>())
            {
                if (department.Status)
                {
                    _itemRepo.AddDepartmentToItem(department.Id, itemId);
                }
            }
            return itemId;
        }

        private string UploadedImageSave(IFormFile imageObj)
        {
            string uniqueFileName = null;

            if (imageObj != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageObj.FileName;
                string filePath = Path.Combine(_appSettingPropertyRepo.GetImagesDestinationPath(), uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageObj.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public bool UpdateItem(ItemVM input)
        {
            var item = _itemRepo.GetItem(input.Id);

            item.Name = input.Name;
            item.Description = input.Description;

            if (input.ImageObj != null)
            {
                DeleteOldItemImage(item.ImagePath);
                item.ImagePath = UploadedImageSave(input.ImageObj);
            }

            var itemDepartments = _itemRepo.GetDepartmentListByItem(input.Id);

            foreach (var department in _departmentRepo.GetDepartmentsList())
            {
                if (input.DepartmentsList.Any(x => x.Id == department.Id.ToString().ToLower()))
                {
                    if (!itemDepartments.Any(x => x.Id == department.Id))
                    {
                        _itemRepo.AddDepartmentToItem(department.Id, item.Id);
                    }
                }
                else
                {
                    if (itemDepartments.Any(x => x.Id == department.Id))
                    {
                        _itemRepo.RemoveDepartmentFromItem(department.Id, item.Id);
                    }
                }
            }



            foreach (var serial in _itemRepo.GetSerialItemsListByItem(item.Id))
            {
                if (!input.SerialsList.Any(x => x.Id == serial.Id.ToString().ToLower()))
                {
                    _itemRepo.DeleteSerial(serial);
                }
            }

            var buffSerialsItem = _itemRepo.GetSerialItemsListByItem(item.Id);

            foreach (var serial in input.SerialsList)
            {
                if (!buffSerialsItem.Any(x => x.Id.ToString().ToLower() == serial.Id))
                {
                    _itemRepo.AddSerialToItem(new SerialItem { SerialNumber = serial.Name }, item);
                }
            }

            return _itemRepo.UpdateItem(item);
        }

        public bool DeleteItem(string id)
        {
            foreach (var reservation in _itemRepo.GetItemReservationListByItem(id))
            {
                _reservationRepo.RemoveReservationById(reservation.Id);
            }

            foreach (var serial in _itemRepo.GetSerialItemsListByItem(id))
            {
                _itemRepo.DeleteSerial(serial);
            }

            var item = _itemRepo.GetItem(id);

            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                DeleteOldItemImage(item.ImagePath);
            }

            return _itemRepo.DeleteItem(item);
        }

        private bool DeleteOldItemImage(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;      
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;        
        }
    }
}
