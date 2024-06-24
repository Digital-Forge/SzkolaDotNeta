using Domain.Models.Business;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Domain.Interfaces.Repositories.IReservationRepository;

namespace Infrastructure.Repositories
{
    public partial class ReservationRepository
    {
        private class ReservationQuery : IReservationQuery
        {
            private IQueryable<Reservation> _query;

            public ReservationQuery(Context context, bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false)
            {
                OnlyActive = onlyActive;
                AsNoTracking = asNoTracking;
                AllowBuffer = allowBuffor;

                _query = context.Reservations;
            }

            public bool AsNoTracking { get; }

            public bool OnlyActive { get; }

            public bool AllowBuffer { get; }

            public IQueryable<Reservation> Query
            {
                get
                {
                    if (AsNoTracking) _query = _query.AsNoTracking();
                    if (OnlyActive) _query = _query.Where(x => x.Active);

                    return _query.Where(x => !(x.EntityStatus == Domain.Utils.EntityStatus.Delete || (!AllowBuffer && x.EntityStatus == Domain.Utils.EntityStatus.Buffer)));
                }
            }

            public IReservationQuery IncludeItems()
            {
                _query = _query
                    .Include(i => i.ItemInstance)
                    .ThenInclude(i => i.Item);

                return this;
            }

            public IReservationQuery IncludeItemsWithDepartments()
            {
                _query = _query
                    .Include(i => i.ItemInstance)
                    .ThenInclude(i => i.Item)
                    .ThenInclude(i => i.Departments)
                    .ThenInclude(i => i.Department);

                return this;
            }

            public IReservationQuery IncludeUsers()
            {
                _query = _query.Include(i => i.User);

                return this;
            }

            public Reservation? GetReservationById(Guid id)
            {
                return Query.FirstOrDefault(x => x.Id == id);
            }

            public async Task<Reservation?> GetReservationByIdAsync(Guid id)
            {
                return await Query.FirstOrDefaultAsync(x => x.Id == id);
            }

            public List<Reservation> GetReservations()
            {
                return Query.ToList();
            }

            public async Task<List<Reservation>> GetReservationsAsync()
            {
                return await Query.ToListAsync();
            }

            public IReservationQuery Where(Expression<Func<Reservation, bool>> predicate)
            {
                _query = _query.Where(predicate);

                return this;
            }
        };
    }
}

