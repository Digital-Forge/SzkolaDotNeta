using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IReservationRepository))]
    public partial class ReservationRepository(Context _context) : IReservationRepository
    {
        public IReservationRepository.IReservationQuery QueryBuilder(bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false)
        {
            return new ReservationQuery(_context, onlyActive, asNoTracking, allowBuffor);
        }

        public Guid Save(Reservation entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<Reservation>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = _context.Reservations.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id) != null;
                if (isExist) _context.Reservations.Update(entity);
                else _context.Reservations.Add(entity);
            }

            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<Guid> SaveAsync(Reservation entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<Reservation>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = await _context.Reservations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id) != null;
                if (isExist) _context.Reservations.Update(entity);
                else await _context.Reservations.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var entity = _context.Reservations.First(x => x.Id == id);
            _context.Reservations.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Reservations.FirstAsync(x => x.Id == id);
            _context.Reservations.Remove(entity);
            await _context.SaveChangesAsync(); ;
        }
    }
}
