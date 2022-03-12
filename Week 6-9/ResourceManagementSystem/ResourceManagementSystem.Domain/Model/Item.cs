using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class Item
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        //Relations
        public virtual ICollection<SerialItem> Serials { get; set; }
        public virtual ICollection<ItemToDepartment> Departments { get; set; }
        public virtual ICollection<ItemReservation> Reservations { get; set; }
    }
}
