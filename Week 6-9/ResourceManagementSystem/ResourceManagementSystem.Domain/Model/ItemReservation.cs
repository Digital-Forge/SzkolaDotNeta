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
        public int Id { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        //Relations
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int? AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int? SerialItemId { get; set; }
        public virtual SerialItem SerialItem { get; set; }
    }
}
