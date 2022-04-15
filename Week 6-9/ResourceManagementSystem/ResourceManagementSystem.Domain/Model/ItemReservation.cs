using ResourceManagementSystem.Domain.Model.ExtraModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class ItemReservation
    {
        [Key]
        public Guid Id { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        //Relations
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public Guid? SerialItemId { get; set; }
        public virtual SerialItem SerialItem { get; set; }
    }
}
