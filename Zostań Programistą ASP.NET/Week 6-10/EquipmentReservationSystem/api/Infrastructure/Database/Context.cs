using Domain.Models;
using Domain.Models.Business;
using Domain.Models.Business.MiddleTabs;
using Domain.Models.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public partial class Context : IdentityDbContext<UserData, IdentityRole<Guid>, Guid>
    {
        // Business
        public DbSet<UserData> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemInstance> ItemInstances { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ServiceNote> ServiceNotes { get; set; }


        //Middle Tabs
        public DbSet<UserToDepartment> UsersToDepartments { get; set; }
        public DbSet<ItemToDepartment> ItemsToDepartments { get; set; }


        //System
        public DbSet<RefreshToken> RefreshTokens { get; set; } 
        public DbSet<DataFile> Files { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
