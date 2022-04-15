using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class SerialItem
    {
        [Key]
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }

        //Relations
        public Guid IdItem { get; set; }
        public virtual Item Item { get; set; }

        public Guid? ItemReservationId { get; set; }
        public virtual ItemReservation Reservation { get; set; }
    }
}
