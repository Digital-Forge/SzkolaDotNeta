using Domain.Interfaces.Models;
using Domain.Models.Business;
using Domain.Models.Business.MiddleTabs;
using Domain.Utils;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class UserData : IdentityUser<Guid>, IAuditableEntity
    {
        public Guid CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public EntityStatus? EntityStatus { get; set; }
        public bool Active { get; set; }

        //Relations
        public virtual List<UserToDepartment> Departments { get; set; } = new List<UserToDepartment>();
        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
