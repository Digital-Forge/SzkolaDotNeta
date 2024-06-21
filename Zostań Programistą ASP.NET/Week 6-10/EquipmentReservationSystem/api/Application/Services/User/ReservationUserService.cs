using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services.User;
using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.User
{
    [AutoRegisterTransientService(typeof(IReservationUserService))]
    public partial class ReservationUserService(IUserRepository _userRepository, IReservationRepository _reservationRepository, IItemRepository _itemRepository, IFileService _fileService, IAuthService _authService) : IReservationUserService
    {
        public async Task CreateReservationAsync(IReservationUserService.CreateReservationModel model, Guid? userId = null)
        {
            userId ??= _userRepository.GetContextUser().Id;

            var item = await _itemRepository.QueryBuilder(true)
                .IncludeInstances()
                .GetItemByIdAsync(model.ItemId);

            if (item == null) throw new NotFountItemException();

            var instance = item.Instances.FirstOrDefault(x => x.Active && x.Status == Domain.Utils.ItemInstanceStatus.Available) ?? throw new ItemInstanceNotAvailableException();
            instance.Status = Domain.Utils.ItemInstanceStatus.Reservations;

            var entity = new Reservation()
            {
                Active = true,
                From = model.From,
                To = model.To,
                UserId = userId.Value,
                ItemInstanceId = instance.Id,
                Status = Domain.Utils.ReservationStatus.InPreparation,
            };

            await _itemRepository.SaveAsync(instance);
            await _reservationRepository.SaveAsync(entity);
        }

        public async Task<IReservationUserService.ReservationModel> GetReservationAsync(Guid id)
        {
            var entity = await _reservationRepository.QueryBuilder(asNoTracking: true)
                .IncludeItems()
                .GetReservationByIdAsync(id);

            if (_userRepository.GetContextUser().Id != entity.UserId && !await _authService.IsAccessToPickUpPointAsync()) throw new ReservationAccessDeniedException();

            var model = new IReservationUserService.ReservationModel()
            {
                Id = entity.Id,
                Name = entity.ItemInstance.Item.Name,
                Description = entity.ItemInstance.Item.Description ?? "",
                SerialNumber = entity.ItemInstance.SerialNumber,
                Status = entity.Status,
                From = entity.From,
                To = entity.To,
                Images = entity.ItemInstance.Item.Images,
            };

            foreach (var file in entity.ItemInstance.Item.AddictionFiles)
            {
                try
                {
                    var info = await _fileService.GetFileInfoAsync(file);
                    if (info == null || (info.IsMissing ?? false)) continue;

                    model.Files.Add(new IReservationUserService.ReservationModel.ItemFile
                    {
                        Id = info.Id.Value,
                        Name = info.Name,
                    });
                }
                catch
                {
                    continue;
                }
            }

            return model;
        }

        public async Task<IPaginationTable<IReservationUserService.ReservationTableModel>.TableData> GetUserReservationAsync(IReservationUserService.ReservationTableOptions options, Guid? userId = null)
        {
            userId ??= _userRepository.GetContextUser().Id;

            var data = new IPaginationTable<IReservationUserService.ReservationTableModel>.TableData();

            var itemsQuery = _reservationRepository.QueryBuilder(asNoTracking: true, onlyActive: true)
                .IncludeItemsWithDepartments()
                .Where(x => x.UserId == userId)
                .Where(x => x.Status == Domain.Utils.ReservationStatus.Issued
                         || x.Status == Domain.Utils.ReservationStatus.InPreparation
                         || x.Status == Domain.Utils.ReservationStatus.ReadyToPickedUp)
                .Query
                .OrderBy(o => o.From)
                .ThenBy(o => o.To)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.Search)) itemsQuery = itemsQuery.Where(x => x.ItemInstance.Item.Name.Contains(options.Search));

            if (options?.AvailableInDepartments != null && options.AvailableInDepartments.Count != 0)
                itemsQuery = itemsQuery.Where(x => x.ItemInstance.Item.Departments.Any(d => options.AvailableInDepartments.Contains(d.DepartmentId)));

            var total = (await itemsQuery.ToListAsync()).Count();

            itemsQuery = itemsQuery.Skip(options.Skip ?? 0);
            itemsQuery = itemsQuery.Take(options.Take ?? 0);

            data.Data = await itemsQuery.Select(s => new IReservationUserService.ReservationTableModel()
            {
                Id = s.Id,
                From = s.From,
                To = s.To,
                Name = s.ItemInstance.Item.Name,
                ImageId = s.ItemInstance.Item.Images.FirstOrDefault(),
                Status = s.Status,
            }).ToListAsync();

            return data;
        }

        public async Task<IPaginationTable<IReservationUserService.ReservationTableModel>.TableData> GetUserReservationHistoryAsync(IReservationUserService.ReservationTableOptions options, Guid? userId = null)
        {
            userId ??= _userRepository.GetContextUser().Id;

            var data = new IPaginationTable<IReservationUserService.ReservationTableModel>.TableData();

            var itemsQuery = _reservationRepository.QueryBuilder(asNoTracking: true, onlyActive: true)
                .IncludeItemsWithDepartments()
                .Where(x => x.UserId == userId)
                .Query
                .OrderBy(o => o.From)
                .ThenBy(o => o.To)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(options.Search)) itemsQuery = itemsQuery.Where(x => x.ItemInstance.Item.Name.Contains(options.Search));

            if (options?.AvailableInDepartments != null && options.AvailableInDepartments.Count != 0)
                itemsQuery = itemsQuery.Where(x => x.ItemInstance.Item.Departments.Any(d => options.AvailableInDepartments.Contains(d.DepartmentId)));

            var total = (await itemsQuery.ToListAsync()).Count();

            itemsQuery = itemsQuery.Skip(options.Skip ?? 0);
            itemsQuery = itemsQuery.Take(options.Take ?? 0);

            data.Data = await itemsQuery.Select(s => new IReservationUserService.ReservationTableModel()
            {
                Id = s.Id,
                From = s.From,
                To = s.To,
                Name = s.ItemInstance.Item.Name,
                ImageId = s.ItemInstance.Item.Images.FirstOrDefault(),
                Status = s.Status,
            }).ToListAsync();

            return data;
        }
    }
}
