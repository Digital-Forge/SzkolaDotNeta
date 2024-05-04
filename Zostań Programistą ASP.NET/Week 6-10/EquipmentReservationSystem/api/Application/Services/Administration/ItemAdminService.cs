using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Domain.Models.Business.MiddleTabs;
using Microsoft.EntityFrameworkCore;
using static Application.Interfaces.IItemAdminService;

namespace Application.Services.Administration
{
    [AutoRegisterTransientService(typeof(IItemAdminService))]
    public partial class ItemAdminService(IItemRepository _itemRepository, IFileService _fileService) : IItemAdminService
    {
        public async Task<ItemModel> GetModelAsync(Guid id)
        {
            var entity = await _itemRepository.QueryBuilder(asNoTracking: true)
                .IncludeDepartments()
                .IncludeInstancesThenReservationsThenUsers()
                .GetItemByIdAsync(id);

            var model = new ItemModel()
            {
                Id = entity.Id,
                Active = entity.Active,
                Name = entity.Name,
                Description = entity.Description,
                ImagesId = entity.Images,
                Create = DateOnly.FromDateTime(entity.CreateTime),
                Departments = entity.Departments
                    .Select(s => new ItemModel.DepartmentModel()
                    {
                        Id = s.DepartmentId,
                        Name = s.Department.Name,
                    })
                    .OrderBy(o => o.Name)
                    .ToList(),
                Instances = entity.Instances
                    .Select(s => new ItemModel.Instance()
                    {
                        Id = s.Id,
                        Active = s.Active,
                        SerialNumber = s.SerialNumber,
                        Status = s.Status,
                        AddedDate = s.AddedDate,
                        WithdrawalDate = s.WithdrawalDate,
                    })
                    .OrderBy(o => o.AddedDate)
                    .ToList(),
                ReservationsHistory = entity.Instances
                    .SelectMany(s => s.Reservations)
                    .Select(s => new ItemModel.ReservationHistory()
                    {
                        Id = s.Id,
                        SerialNumber = s.ItemInstanceId.ToString(),
                        Who = s.User?.UserName,
                        From = s.From,
                        To = s.To,
                        Status = s.Status,
                    })
                    .OrderByDescending(o => o.From)
                    .ToList(),
            };

            var fileProcessing = entity.AddictionFiles.Select(s => _fileService.GetFileInfoAsync(s)).ToList();

            model.AddictionFiles = (await Task.WhenAll(fileProcessing))
                .Select(s => new ItemModel.AddictionFile()
                {
                    Id = s.Id,
                    Active = s.Active,
                    Name = s.Name,
                    Date = s.Date.Value,
                })
                .OrderBy(o => o.Name)
                .ToList();

            return model;
        }

        public async Task<Guid> CreateAsync(ItemModel model)
        {
            var entity = new Item()
            {
                Active = model.Active,
                Name = model.Name,
                Description = model.Description,
                Images = model.ImagesId,
                AddictionFiles = model.AddictionFiles.Select(s => s.Id.Value).ToList(),
                Departments = model.Departments.Select(s => new ItemToDepartment()
                {
                    DepartmentId = s.Id.Value,
                }).ToList(),
                Instances = model.Instances.Select(s => new ItemInstance()
                {
                    Status = s.Status,
                    Active = s.Active,
                    AddedDate = s.AddedDate,
                    WithdrawalDate = s.WithdrawalDate,
                    SerialNumber = s.SerialNumber,
                }).ToList()
            };

            await _itemRepository.SaveAsync(entity);

            foreach (var file in model.AddictionFiles)
            {
                await FileStatusUpdate(file.Id.Value, file.Active);
            }

            foreach (var imageId in model.ImagesId)
            {
                await FileStatusUpdate(imageId, true);
            }

            return entity.Id;
        }

        public async Task UpdateAsync(ItemModel model)
        {
            var entity = await _itemRepository.QueryBuilder()
                .IncludeInstances()
                .IncludeDepartments()
                .GetItemByIdAsync(model.Id.Value);

            entity.Active = model.Active;
            entity.Name = model.Name;
            entity.Description = model.Description;

            // departments
            entity.Departments = model.Departments.Select(s => new ItemToDepartment()
            {
                DepartmentId = s.Id.Value,
                ItemId = entity.Id,
            }).ToList();

            // instances
            entity.Instances = entity.Instances.Where(x => model.Instances.Any(y => y.Id == x.Id)).ToList();

            foreach (var instance in entity.Instances)
            {
                var instanceModel = model.Instances.FirstOrDefault(x => x.Id == instance.Id);
                if (instanceModel == null) continue;

                instance.Status = instanceModel.Status;
                instance.Active = instanceModel.Active;
                instance.AddedDate = instanceModel.AddedDate;
                instance.WithdrawalDate = instanceModel.WithdrawalDate;
                instance.SerialNumber = instanceModel.SerialNumber;
            }

            foreach (var instance in model.Instances.Where(x => x.Id == null))
            {
                entity.Instances.Add(new ItemInstance()
                {
                    Status = instance.Status,
                    Active = instance.Active,
                    AddedDate = instance.AddedDate,
                    WithdrawalDate = instance.WithdrawalDate,
                    SerialNumber = instance.SerialNumber,
                });
            }

            // images
            foreach (var imageId in entity.Images)
            {
                await FileStatusUpdate(imageId, false, true);
            }

            entity.Images = model.ImagesId;

            foreach (var imageId in entity.Images)
            {
                await FileStatusUpdate(imageId, true);
            }

            // files
            foreach (var fileId in entity.AddictionFiles)
            {
                await FileStatusUpdate(fileId, false, true);
            }

            entity.AddictionFiles = model.AddictionFiles.Select(s => s.Id.Value).ToList();

            foreach (var file in model.AddictionFiles)
            {
                await FileStatusUpdate(file.Id.Value, file.Active);
            }

            await _itemRepository.SaveAsync(entity);
        }

        private async Task FileStatusUpdate(Guid id, bool? active, bool isTemporary = false)
        {
            var info = await _fileService.GetFileInfoAsync(id);
            if (active != null) info.Active = active.Value;
            await _fileService.UpdateFileInfo(info, isTemporary);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _itemRepository.QueryBuilder(asNoTracking: true)
                .GetItemByIdAsync(id);

            foreach (var fileId in entity.AddictionFiles)
            {
                await FileStatusUpdate(fileId, null, true);
            }
            foreach (var imageId in entity.Images)
            {
                await FileStatusUpdate(imageId, null, true);
            }

            await _itemRepository.DeleteAsync(id);
        }

        public IPaginationTable<ItemTableModel>.TableData GetTable(IPaginationTable<ItemTableModel>.TableOptions options)
        {
            var data = new IPaginationTable<ItemTableModel>.TableData();

            var query = _itemRepository.QueryBuilder(asNoTracking: true)
                .IncludeInstances()
                .IncludeDepartments()
                .Query;

            if (!string.IsNullOrWhiteSpace(options.Search)) query = query.Where(x => x.Name.Contains(options.Search));

            if (options is ItemTableOprtion)
            {
                var localOption = options as ItemTableOprtion;
                if (localOption != null && localOption.AvailableInDepartments.Count != 0)
                    query = query.Where(x => x.Departments.Any(d => localOption.AvailableInDepartments.Contains(d.DepartmentId)));
            }

            data.TotalCount = query.Count();
            query = query.OrderBy(o => o.Name);
            query = query.Skip(options.Skip ?? 0);
            query = query.Take(options.Take ?? 0);

            data.Data = query.ToList().Select(s => new ItemTableModel
            {
                Id = s.Id,
                Name = s.Name,
                Active = s.Active,
                Count = s.Instances.Count,
                ImageId = s.Images.FirstOrDefault().ToString(),
            }).ToList();

            return data;
        }

        public async Task<IPaginationTable<ItemTableModel>.TableData> GetTablAsync(IPaginationTable<ItemTableModel>.TableOptions options)
        {
            var data = new IPaginationTable<ItemTableModel>.TableData();

            var query = _itemRepository.QueryBuilder(asNoTracking: true)
                .IncludeInstances()
                .IncludeDepartments()
                .Query;

            if (!string.IsNullOrWhiteSpace(options.Search)) query = query.Where(x => x.Name.Contains(options.Search));

            if (options is ItemTableOprtion)
            {
                var localOption = options as ItemTableOprtion;
                if (localOption != null && localOption.AvailableInDepartments.Count != 0)
                    query = query.Where(x => x.Departments.Any(d => localOption.AvailableInDepartments.Contains(d.DepartmentId)));
            }

            data.TotalCount = await query.CountAsync();
            query = query.OrderBy(o => o.Name);
            query = query.Skip(options.Skip ?? 0);
            query = query.Take(options.Take ?? 0);

            data.Data = (await query.ToListAsync()).Select(s => new ItemTableModel
            {
                Id = s.Id,
                Name = s.Name,
                Active = s.Active,
                Count = s.Instances.Count,
                ImageId = s.Images.FirstOrDefault().ToString(),
            }).ToList();

            return data;
        }
    }
}
