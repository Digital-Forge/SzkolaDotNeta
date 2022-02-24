using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public bool isActive { get; set; }
        [Required]
        public bool isFirstAccess { get; set; }

        //Relations
        public virtual ICollection<UserToDepartment> Departments { get; set; }
        public virtual ICollection<ItemReservation> Reservations { get; set; }
    }
}
