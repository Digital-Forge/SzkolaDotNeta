namespace Domain.Models.Business
{
    public class ItemInstance : AuditableEntity
    {
        public string SerialNumber { get; set; }
        public Guid Starus { get; set; }
        public Guid DurabilityStatus { get; set; }
        public DateOnly AddedDate { get; set; }
        public DateOnly WithdrawalDate { get; set; }

        //Relations
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
