using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly Context _context;

        public ReservationRepository(Context context)
        {
            _context = context;
        }


        public IQueryable<ItemReservation> GetReservationListByUser(string userId)
        {
            return _context.ItemReservations.Where(x => x.AppUser.Id == userId);
        }
    }
}
