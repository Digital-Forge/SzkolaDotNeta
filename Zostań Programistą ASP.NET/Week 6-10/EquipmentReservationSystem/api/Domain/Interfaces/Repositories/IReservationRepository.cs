using Domain.Interfaces.Repositories.Abstract;
using Domain.Models.Business;
using static Domain.Interfaces.Repositories.IReservationRepository;

namespace Domain.Interfaces.Repositories
{
    public interface IReservationRepository : IRepository<IReservationQuery, Reservation>
    {
        interface IReservationQuery : IRepositoryQueryBuilder<Reservation>
        {
            IReservationQuery IncludeItems();
            IReservationQuery IncludeUsers();
            IReservationQuery IncludeItemsWithDepartments();
            IReservationQuery Where(System.Linq.Expressions.Expression<Func<Reservation, bool>> predicate);
            Reservation? GetReservationById(Guid id);
            Task<Reservation?> GetReservationByIdAsync(Guid id);
            List<Reservation> GetReservations();
            Task<List<Reservation>> GetReservationsAsync();
        }
    }
}
