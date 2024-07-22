using Domain.Models.Business;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Application.Constans.Constans.Role;
using static Domain.Interfaces.Repositories.IItemRepository;

namespace Infrastructure.Repositories
{
    public partial class ItemRepository
    {
        private class ItemQuery : IItemQuery
        {
            public ItemQuery(Context context, bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false)
            {
                OnlyActive = onlyActive;
                AsNoTracking = asNoTracking;
                AllowBuffer = allowBuffor;

                _query = context.Items;
            }

            public bool AsNoTracking { get; }

            public bool OnlyActive { get; }

            public bool AllowBuffer { get; }

            private IQueryable<Item> _query;
            public IQueryable<Item> Query
            {
                get
                {
                    if (AsNoTracking) _query = _query.AsNoTracking();
                    if (OnlyActive) _query = _query.Where(x => x.Active);

                    return _query.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer)));
                }
            }

            public IItemQuery IncludeDepartments()
            {
                _query = _query
                    .Include(x => x.Departments.Where(x => !(x.Department.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.Department.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(x => x.Department);
                return this;
            }

            public IItemQuery IncludeInstances()
            {
                _query = _query.Include(x => x.Instances.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))));
                return this;
            }

            public IItemQuery IncludeInstancesThenReservations()
            {
                _query = _query
                    .Include(x => x.Instances.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(x => x.Reservations.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))));
                return this;
            }

            public IItemQuery IncludeInstancesThenReservationsThenUsers()
            {
                _query = _query
                    .Include(x => x.Instances.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(x => x.Reservations.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer))))
                    .ThenInclude(x => x.User);
                return this;
            }

            public Item? GetItemById(Guid id)
            {
                return Query.FirstOrDefault(x => x.Id == id);
            }

            public async Task<Item?> GetItemByIdAsync(Guid id)
            {
                return await Query.FirstOrDefaultAsync(x => x.Id == id);
            }

            public Item? GetItemByName(string name)
            {
                return Query.FirstOrDefault(x => x.Name == name);
            }

            public async Task<Item?> GetItemByNameAsync(string name)
            {
                return await Query.FirstOrDefaultAsync(x => x.Name == name);
            }

            public List<Item> GetItems()
            {
                return Query.ToList();
            }

            public async Task<List<Item>> GetItemsAsync()
            {
                return await Query.ToListAsync();
            }

            public IItemQuery Where(Expression<Func<Item, bool>> predicate)
            {
                _query = _query.Where(predicate);
                return this;
            }

            public Item? GetItemByInstanceId(Guid id)
            {
                return Query.FirstOrDefault(x => x.Instances.Any(y => y.Id == id));
            }

            public async Task<Item?> GetItemByInstanceIdAsync(Guid id)
            {
                return await Query.FirstOrDefaultAsync(x => x.Instances.Any(y => y.Id == id));
            }

            public Item? GetItemBySerialNumber(string serialNumber)
            {
                return Query.FirstOrDefault(x => x.Instances.Any(y => y.SerialNumber.Equals(serialNumber, StringComparison.OrdinalIgnoreCase)));
            }

            public async Task<Item?> GetItemBySerialNumberAsync(string serialNumber)
            {
                return await Query.FirstOrDefaultAsync(x => x.Instances.Any(y => y.SerialNumber.Equals(serialNumber, StringComparison.OrdinalIgnoreCase)));
            }
        }
    }
}
