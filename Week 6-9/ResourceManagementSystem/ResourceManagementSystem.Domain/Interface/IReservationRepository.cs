using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Domain.Interface
{
    public interface IReservationRepository
    {
        IQueryable<ItemReservation> GetReservationListByUser(string userId);
        bool RemoveReservationById(Guid id);
    }
}
