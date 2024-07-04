using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services.PickupPoint;
using Domain.Interfaces.Repositories;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.PickupPoint
{
    [AutoRegisterTransientService(typeof(IReservationPickupPointService))]
    public partial class ReservationPickupPointService(IReservationRepository _reservationRepository, IFileService _fileService, IItemRepository _itemRepository, IUserRepository _userRepository, IAuthService _authService) : IReservationPickupPointService
    {
        public async Task ChangeReservationStatusAsync(IReservationPickupPointService.ChangeReservationModel model)
        {
            if (model.AdminMode && !await _authService.IsUserAdminAsync()) throw new AdminAccessException();

            var entity = _reservationRepository.QueryBuilder()
                .IncludeItems()
                .GetReservationById(model.ReservationId);

            if (model.From != null)
            {
                entity.From = model.From.Value;
                entity.To = model.To;
            }

            if (!string.IsNullOrWhiteSpace(model.InnerNote))
            {
                entity.InnerNote += $"+++ Updata: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}\n{model.InnerNote}\n\n";

                if (entity.InnerNote.Length > 3000) throw new InnerNoteLimitException();
            }

            switch (model.Status)
            {
                case ReservationStatus.InPreparation:
                    if (!model.AdminMode) throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.InPreparation;
                    entity.ItemInstance.Status = ItemInstanceStatus.Reservations;
                    break;
                case ReservationStatus.ReadyToPickedUp:
                    if (entity.Status != ReservationStatus.InPreparation && !model.AdminMode)
                        throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.ReadyToPickedUp;
                    entity.ItemInstance.Status = ItemInstanceStatus.Reservations;
                    break;
                case ReservationStatus.Issued:
                    if (entity.Status != ReservationStatus.ReadyToPickedUp && !model.AdminMode)
                        throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.Issued;
                    entity.ItemInstance.Status = ItemInstanceStatus.Reservations;
                    break;
                case ReservationStatus.Returned:
                    if (entity.Status != ReservationStatus.Issued && !model.AdminMode)
                        throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.Returned;
                    entity.ItemInstance.Status = ItemInstanceStatus.Available;
                    break;
                case ReservationStatus.Lost:
                    if (entity.Status != ReservationStatus.Issued && !model.AdminMode)
                        throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.Lost;
                    entity.ItemInstance.Status = ItemInstanceStatus.Unavailable;
                    entity.ItemInstance.Active = false;
                    break;
                case ReservationStatus.Destroyed:
                    if (entity.Status != ReservationStatus.Issued && !model.AdminMode)
                        throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.Destroyed;
                    entity.ItemInstance.Status = ItemInstanceStatus.Service;
                    break;
                case ReservationStatus.Canceled:
                    if (!(entity.Status == ReservationStatus.InPreparation || entity.Status == ReservationStatus.ReadyToPickedUp) && !model.AdminMode)
                        throw new ReservationChangeStatusException(model.Status, entity.Status);

                    entity.Status = ReservationStatus.Canceled;
                    entity.ItemInstance.Status = ItemInstanceStatus.Available;
                    break;
                default:
                    throw new NotImplementedException($"ChangeReservationStatus does not support this status ({model.Status}).");
            }

            _reservationRepository.Save(entity);
        }

        public async Task<IPaginationTable<IReservationPickupPointService.ReservationTableModel>.TableData> GetItemsToReturnAsync(IReservationPickupPointService.ReturnsReservationsTableOption options)
        {
            var data = new IPaginationTable<IReservationPickupPointService.ReservationTableModel>.TableData();

            var itemsQuery = _reservationRepository.QueryBuilder(asNoTracking: true, onlyActive: true)
                .IncludeUsers()
                .IncludeItems()
                .Where(x => x.Status == ReservationStatus.Issued)
                .Query
                .OrderBy(o => o.To)
                .AsQueryable();

            if (options?.SearchItems != null && options.SearchItems.Count != 0)
                itemsQuery = itemsQuery.Where(x => options.SearchItems.Any(si => x.ItemInstance.Item.Id == si));

            if (options?.SearchUsers != null && options.SearchUsers.Count != 0)
                itemsQuery = itemsQuery.Where(x => options.SearchUsers.Any(su => x.UserId == su));

            var total = (await itemsQuery.ToListAsync()).Count();

            itemsQuery = itemsQuery.Skip(options.Skip ?? 0);
            itemsQuery = itemsQuery.Take(options.Take ?? 0);

            data.Data = await itemsQuery.Select(s => new IReservationPickupPointService.ReservationTableModel()
            {
                Id = s.Id,
                From = s.From,
                To = s.To,
                Status = s.Status,
                ItemName = s.ItemInstance.Item.Name,
                Username = s.User.UserName,
                Image = s.ItemInstance.Item.Images.FirstOrDefault(),
            }).ToListAsync();

            return data;
        }

        public async Task<IPaginationTable<IReservationPickupPointService.ReservationTableModel>.TableData> GetPreparationAndReleaseReservationsAsync(IReservationPickupPointService.PreparationAndReleaseReservationsTableOption options)
        {
            var data = new IPaginationTable<IReservationPickupPointService.ReservationTableModel>.TableData();

            var whatShow = options.ShowType switch
            {
                IReservationPickupPointService.PreparationAndReleaseReservationsTableOption.ReservationShowType.All =>
                     new List<ReservationStatus>() { ReservationStatus.InPreparation, ReservationStatus.ReadyToPickedUp },
                IReservationPickupPointService.PreparationAndReleaseReservationsTableOption.ReservationShowType.InPreparation =>
                     new List<ReservationStatus>() { ReservationStatus.InPreparation },
                IReservationPickupPointService.PreparationAndReleaseReservationsTableOption.ReservationShowType.ReadyToPickedUp =>
                     new List<ReservationStatus>() { ReservationStatus.ReadyToPickedUp },
                _ => new List<ReservationStatus>()
            };

            var itemsQuery = _reservationRepository.QueryBuilder(asNoTracking: true, onlyActive: true)
                .IncludeUsers()
                .IncludeItems()
                .Where(x => whatShow.Contains(x.Status))
                .Query
                .OrderBy(o => o.Status == ReservationStatus.InPreparation)
                .ThenBy(o => o.From)
                .ThenBy(o => o.To)
                .AsQueryable();

            if (options?.SearchItems != null && options.SearchItems.Count != 0)
                itemsQuery = itemsQuery.Where(x => options.SearchItems.Any(si => x.ItemInstance.Item.Id == si));

            if (options?.SearchUsers != null && options.SearchUsers.Count != 0)
                itemsQuery = itemsQuery.Where(x => options.SearchUsers.Any(su => x.UserId == su));

            var total = (await itemsQuery.ToListAsync()).Count();

            itemsQuery = itemsQuery.Skip(options.Skip ?? 0);
            itemsQuery = itemsQuery.Take(options.Take ?? 0);

            data.Data = await itemsQuery.Select(s => new IReservationPickupPointService.ReservationTableModel()
            {
                Id = s.Id,
                From = s.From,
                To = s.To,
                Status = s.Status,
                ItemName = s.ItemInstance.Item.Name,
                Username = s.User.UserName,
                Image = s.ItemInstance.Item.Images.FirstOrDefault(),
            }).ToListAsync();

            return data;
        }

        public async Task<IReservationPickupPointService.ReservationModel> GetReservationInfoAsync(Guid id)
        {
            var entity = await _reservationRepository.QueryBuilder(asNoTracking: true)
                .IncludeItems()
                .GetReservationByIdAsync(id);

            var model = new IReservationPickupPointService.ReservationModel()
            {
                Id = entity.Id,
                Name = entity.ItemInstance.Item.Name,
                Description = entity.ItemInstance.Item.Description ?? "",
                SerialNumber = entity.ItemInstance.SerialNumber,
                Status = entity.Status,
                From = entity.From,
                To = entity.To,
                Images = entity.ItemInstance.Item.Images,
                InnerNote = entity.InnerNote,
            };

            foreach (var file in entity.ItemInstance.Item.AddictionFiles)
            {
                try
                {
                    var info = await _fileService.GetFileInfoAsync(file);
                    if (info == null || info.Active == false) continue;

                    model.Files.Add(new IReservationPickupPointService.ReservationModel.FileInfoModel()
                    {
                        Id = info.Id.Value,
                        Name = info.Name,
                        IsMissing = info.IsMissing ?? false,
                    });
                }
                catch
                {
                    continue;
                }
            }

            return model;
        }

        public async Task<IComboBoxApi<Guid>.ResponsData> GetReservedItemWithStatusAsync(IComboBoxApi<Guid>.SearchOption search, params ReservationStatus[] status)
        {
            var toRespons = new IComboBoxApi<Guid>.ResponsData();

            var reservedItems = await _reservationRepository.QueryBuilder(true, true)
                .IncludeItems()
                .Where(x => status.Contains(x.Status))
                .Query
                .Select(s => s.ItemInstance.Item.Id)
                .Distinct()
                .ToListAsync();

            var queryItem = _itemRepository.QueryBuilder(true, true)
                .Where(x => reservedItems.Contains(x.Id))
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

        public async Task<IComboBoxApi<Guid>.ResponsData> GetReservedUserWithStatusAsync(IComboBoxApi<Guid>.SearchOption search, params ReservationStatus[] status)
        {
            var toRespons = new IComboBoxApi<Guid>.ResponsData();

            var reservedItems = await _reservationRepository.QueryBuilder(true, true)
                .Where(x => status.Contains(x.Status))
                .Query
                .Select(s => s.UserId)
                .Distinct()
                .ToListAsync();

            var queryItem = _userRepository.QueryBuilder(true, true)
                .Where(x => reservedItems.Contains(x.Id))
                .Query;

            toRespons.Data = await queryItem
               .Where(x => x.UserName.ToLower().Contains(search.Search.ToLower()))
               .OrderBy(x => x.UserName)
               .Take(search.Take ?? int.MaxValue)
               .Select(s => new IComboBoxApi<Guid>.ResponsData.PositionData()
               {
                   Code = s.Id,
                   Name = s.UserName,
               }).ToListAsync();

            if ((search.SerchingPosition?.Count ?? 0) != 0)
                toRespons.SerchingPosition = await queryItem
                    .Where(x => search.SerchingPosition.Contains(x.Id))
                    .Select(s => new IComboBoxApi<Guid>.ResponsData.PositionData()
                    {
                        Code = s.Id,
                        Name = s.UserName,
                    }).ToListAsync();

            return toRespons;
        }
    }
}
