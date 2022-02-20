using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class SerialItem
    {
        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public bool ActiveStatus { get; set; }

        //Relations
        public int IdItem { get; set; }
        public virtual Item Item { get; set; }

        public int? ItemReservationId { get; set; }
        public virtual ItemReservation Reservation { get; set; }
    }
}
