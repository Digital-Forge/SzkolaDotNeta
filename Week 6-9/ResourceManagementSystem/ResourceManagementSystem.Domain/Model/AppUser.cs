using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class AppUser : IdentityUser
    {
        //Relations
        public virtual ICollection<UserToDepartment> Departments { get; set; }
        public virtual ICollection<ItemReservation> Reservations { get; set; }
    }
}
