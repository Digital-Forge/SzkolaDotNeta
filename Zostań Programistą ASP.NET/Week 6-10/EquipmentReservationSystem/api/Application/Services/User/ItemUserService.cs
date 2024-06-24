using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services.User;
using Domain.Interfaces.Repositories;

namespace Application.Services.User
{
    [AutoRegisterTransientService(typeof(IItemUserService))]
    public partial class ItemUserService(IItemRepository _itemRepository, IUserRepository _userRepository, IFileService _fileService) : IItemUserService
    {
        public async Task CheckAvailableItemAsync(Guid id)
        {
            var entity = await _itemRepository.QueryBuilder(true, true)
                .IncludeInstances()
                .GetItemByIdAsync(id);

            if (entity == null || !entity.Instances.Any(x => x.Status == Domain.Utils.ItemInstanceStatus.Available && x.Active)) throw new ItemInstanceNotAvailableException();
        }

        public async Task<IItemUserService.ItemModel> GetItemAsync(Guid id)
        {
            var entity = await _itemRepository.QueryBuilder(true, true)
                .IncludeInstances()
                .GetItemByIdAsync(id);

            if (entity == null || !entity.Instances.Any(x => x.Status == Domain.Utils.ItemInstanceStatus.Available && x.Active)) throw new ItemInstanceNotAvailableException();

            var model = new IItemUserService.ItemModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Images = entity.Images,
            };

            foreach (var file in entity.AddictionFiles)
            {
                try
                {
                    var info = await _fileService.GetFileInfoAsync(file);
                    if (info == null || (info.IsMissing ?? false)) continue;

                    model.Files.Add(new IItemUserService.ItemModel.FileInfoModel()
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

        public async Task<IPaginationTable<IItemUserService.ItemTableModel>.TableData> GetUserAvailableItemsAsync(IItemUserService.ItemTableOption options, Guid? userId = null)
        {
            userId ??= _userRepository.GetContextUser().Id;

            var data = new IPaginationTable<IItemUserService.ItemTableModel>.TableData();

            var departmentsAccess = _userRepository.GetUserDepartments(userId.Value).Select(s => s.Id).ToList();

            DateOnly nowData = DateOnly.FromDateTime(DateTime.Now);

            var itemsQuery = _itemRepository.QueryBuilder(true, true)
                .IncludeInstances()
                .Where(x => x.Departments.Any(y => departmentsAccess.Contains(y.DepartmentId)))
                .Where(x => x.Instances.Any(y => y.Status == Domain.Utils.ItemInstanceStatus.Available && y.AddedDate <= nowData && (y.WithdrawalDate == null || y.WithdrawalDate > nowData)))
                .Query
                .OrderBy(o => o.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.Search)) itemsQuery = itemsQuery.Where(x => x.Name.Contains(options.Search));

            if (options?.AvailableInDepartments != null && options.AvailableInDepartments.Count != 0)
                itemsQuery = itemsQuery.Where(x => x.Departments.Any(d => options.AvailableInDepartments.Contains(d.DepartmentId)));

            data.TotalCount = itemsQuery.Count();

            itemsQuery = itemsQuery.Skip(options.Skip ?? 0);
            itemsQuery = itemsQuery.Take(options.Take ?? 0);

            data.Data = itemsQuery.Select(s => new IItemUserService.ItemTableModel()
            {
                Id = s.Id,
                Name = s.Name,
                ImageId = s.Images.FirstOrDefault(),
                AvailableQuantity = s.Instances.Count(x => x.Status == Domain.Utils.ItemInstanceStatus.Available && x.AddedDate <= nowData && (x.WithdrawalDate == null || x.WithdrawalDate > nowData))
            }).ToList();

            return data;
        }
    }
}
