using Domain.Utils;

namespace Domain.Models.Business
{
    public class Reservation : AuditableEntity
    {
        public DateOnly From { get; set; }
        public DateOnly? To { get; set; }
        public ReservationStatus Status { get; set; }

        //Relations
        public Guid ItemInstanceId { get; set; }
        public ItemInstance ItemInstance { get; set; }
        public Guid UserId { get; set; }
        public UserData User { get; set; }
    }
}
