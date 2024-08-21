using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services.PickupPoint;
using Domain.Interfaces.Repositories;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Services.PickupPoint
{
    [AutoRegisterTransientService(typeof(IServicePickupPointService))]
    public partial class ServicePickupPointService(IItemRepository _itemRepository, IFileService _fileService) : IServicePickupPointService
    {
        public async Task<IServicePickupPointService.ServiceItemModel> GetServiceItemAsync(Guid id)
        {
            var entity = await _itemRepository.QueryBuilder(asNoTracking: true)
                .IncludeInstancesThenReservationsThenUsers()
                .GetItemByInstanceIdAsync(id);

            var entityInstance = entity.Instances.First(x => x.Id == id);
            var lastReservation = entityInstance.Reservations.OrderBy(o => o.From).LastOrDefault();

            var model = new IServicePickupPointService.ServiceItemModel()
            {
               Id = entityInstance.Id,
               SerialNumber = entityInstance.SerialNumber,
               Name = entity.Name,
               Description = entity.Description,
               Images = entity.Images,
               ServiceNoteList = await _itemRepository.GetAllServiceNoteForItemAsync(id),
               LastReservationInfo = new IServicePickupPointService.ServiceItemModel.LastReservation
               {
                   From = lastReservation.From,
                   To = lastReservation.To,
                   Username = lastReservation.User.UserName
               },
            };

            model.Files = entity.AddictionFiles.Select(s => _fileService.GetFileInfoAsync(s).Result).ToList();

            return model;
        }

        public async Task UpdateItemAsync(IServicePickupPointService.ChangeStatusServiceItemModel model)
        {
            switch (model.Status)
            {
                case IServicePickupPointService.ChangeStatusServiceItemModel.ServiceStatus.Serviced:
                    await UpdateByServicedStatus(model);
                    break;
                case IServicePickupPointService.ChangeStatusServiceItemModel.ServiceStatus.Repaired:
                    await UpdateByRepairedStatus(model);
                    break;
                case IServicePickupPointService.ChangeStatusServiceItemModel.ServiceStatus.Destroyed:
                    await UpdateByDestroyedStatus(model);
                    break;
                default: throw new NotImplementedException();
            }
        }

        private async Task UpdateByServicedStatus(IServicePickupPointService.ChangeStatusServiceItemModel model)
        {
            await UpdateServiceNote(model);
        }

        private async Task UpdateByRepairedStatus(IServicePickupPointService.ChangeStatusServiceItemModel model)
        {
            await UpdateServiceNote(model);

            var entity = (await _itemRepository.QueryBuilder()
                .IncludeInstances()
                .GetItemByInstanceIdAsync(model.Id))
                .Instances.First(x => x.Id == model.Id);

            entity.Status = ItemInstanceStatus.Available;

            await _itemRepository.SaveAsync(entity);
        }

        private async Task UpdateByDestroyedStatus(IServicePickupPointService.ChangeStatusServiceItemModel model)
        {
            await UpdateServiceNote(model);

            var entity = (await _itemRepository.QueryBuilder()
                .IncludeInstances()
                .GetItemByInstanceIdAsync(model.Id))
                .Instances.First(x => x.Id == model.Id);

            entity.Active = false;
            entity.WithdrawalDate = DateOnly.FromDateTime(DateTime.Now);
            entity.Status = ItemInstanceStatus.Unavailable;

            await _itemRepository.SaveAsync(entity);
        }

        private async Task UpdateServiceNote(IServicePickupPointService.ChangeStatusServiceItemModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.ServiceNote))
                await _itemRepository.AddServiceNoteToItemAsync(model.Id, $"+++ {model.Status}: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}\n{model.ServiceNote}\n\n");
        }

        public async Task<IComboBoxApi<Guid>.ResponsData> GetAvailableServiceItemAsync(IComboBoxApi<Guid>.SearchOption search)
        {
            var toRespons = new IComboBoxApi<Guid>.ResponsData();

            var queryItem = _itemRepository.QueryBuilder(true, true)
                .IncludeInstances()
                .Where(x => x.Instances.Any(y => y.Status == ItemInstanceStatus.Service && y.Active))
                .Query;

            toRespons.Data = await queryItem
               .Where(x => x.Name.ToLower().Contains(search.Search.ToLower()))
               .OrderBy(x => x.Name)
               .Take(search.Take ?? int.MaxValue)
               .Select(s => new IComboBoxApi<Guid>.ResponsData.PositionData()
               {
                   Code = s.Id,
                   Name = s.Name,
               }).ToListAsync();

            if ((search.SerchingPosition?.Count ?? 0) != 0)
                toRespons.SerchingPosition = await queryItem
                    .Where(x => search.SerchingPosition.Contains(x.Id))
                    .Select(s => new IComboBoxApi<Guid>.ResponsData.PositionData()
                    {
                        Code = s.Id,
                        Name = s.Name,
                    }).ToListAsync();

            return toRespons;
        }

        public async Task<IPaginationTable<IServicePickupPointService.ServiceItemTableModel>.TableData> GetServiceItemsAsync(IServicePickupPointService.ServiceItemTableOption options)
        {
            var data = new IPaginationTable<IServicePickupPointService.ServiceItemTableModel>.TableData();

            var itemsQuery = _itemRepository.QueryBuilder(asNoTracking: true, onlyActive: true)
                .IncludeInstances()
                .Query
                .SelectMany(s => s.Instances)
                .Where(x => x.Status == ItemInstanceStatus.Service && x.Active)
                .OrderBy(o => o.Item.Name)
                .AsQueryable();

            if (options?.SearchItems != null && options.SearchItems.Count != 0)
                itemsQuery = itemsQuery.Where(x => options.SearchItems.Any(si => x.Item.Id == si));

            if (!string.IsNullOrWhiteSpace(options.Search)) itemsQuery = itemsQuery.Where(x => x.SerialNumber.ToLower().Contains(options.Search.ToLower()));

            var total = (await itemsQuery.ToListAsync()).Count();

            itemsQuery = itemsQuery.Skip(options.Skip ?? 0);
            itemsQuery = itemsQuery.Take(options.Take ?? 0);

            data.Data = await itemsQuery.Select(s => new IServicePickupPointService.ServiceItemTableModel()
            {
                Id = s.Id,
                Name = s.Item.Name,
                SerialNumber = s.SerialNumber,
                ImageId = s.Item.Images.FirstOrDefault()
            }).ToListAsync();

            return data;
        }
    }
}
