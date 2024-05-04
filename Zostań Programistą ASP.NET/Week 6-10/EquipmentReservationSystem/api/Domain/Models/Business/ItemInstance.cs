using Domain.Utils;

namespace Domain.Models.Business
{
    public class ItemInstance : AuditableEntity
    {
        public string SerialNumber { get; set; }
        public ItemInstanceStatus Status { get; set; }
        public DateOnly AddedDate { get; set; }
        public DateOnly? WithdrawalDate { get; set; }

        //Relations
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
