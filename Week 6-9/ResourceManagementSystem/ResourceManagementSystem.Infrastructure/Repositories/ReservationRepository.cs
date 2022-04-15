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

        public bool RemoveReservationById(string id)
        {
            return RemoveReservationById(new Guid(id));
        }

        public bool RemoveReservationById(Guid id)
        {
            var buff = _context.ItemReservations.Where(x => x.Id == id).FirstOrDefault();

            if (buff == null) return false;

            try
            {
                _context.ItemReservations.Remove(buff);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
