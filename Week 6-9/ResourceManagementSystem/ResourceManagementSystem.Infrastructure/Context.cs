using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResourceManagementSystem.Domain.Model;

namespace ResourceManagementSystem.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SerialItem> SerialItems { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ItemReservation> ItemReservations { get; set; }
        public DbSet<ItemToDepartment> ItemsToDepartments { get; set; }
        public DbSet<UserToDepartment> UsersToDepartments { get; set; }


        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //////////////////////////////////////// AppUser Unique Index
            builder.Entity<AppUser>()
                .HasIndex(x => x.Id)
                .IsUnique();

            //////////////////////////////////////// SerialItem many to one Item
            builder.Entity<SerialItem>()
                .HasOne<Item>(x => x.Item)
                .WithMany(y => y.Serials)
                .HasForeignKey(z => z.IdItem);

            //////////////////////////////////////// Item many to many Department
            builder.Entity<ItemToDepartment>().HasKey(k => new { k.ItemId, k.DepartmentId });

            builder.Entity<ItemToDepartment>()
                .HasOne<Item>(x => x.Item)
                .WithMany(y => y.Departments)
                .HasForeignKey(z => z.ItemId);


            builder.Entity<ItemToDepartment>()
                .HasOne<Department>(x => x.Department)
                .WithMany(y => y.Items)
                .HasForeignKey(z => z.DepartmentId);

            //////////////////////////////////////// AppUser many to many Department
            builder.Entity<UserToDepartment>().HasKey(k => new { k.AppUserId, k.DepartmentId });

            builder.Entity<UserToDepartment>()
                .HasOne<AppUser>(x => x.AppUser)
                .WithMany(y => y.Departments)
                .HasForeignKey(z => z.AppUserId);


            builder.Entity<UserToDepartment>()
                .HasOne<Department>(x => x.Department)
                .WithMany(y => y.AppUsers)
                .HasForeignKey(z => z.DepartmentId);

            //////////////////////////////////////// ItemReservation many to one AppUser
            builder.Entity<ItemReservation>()
                .HasOne<AppUser>(x => x.AppUser)
                .WithMany(y => y.Reservations)
                .HasForeignKey(z => z.AppUserId);

            //////////////////////////////////////// ItemReservation many to one Item
            builder.Entity<ItemReservation>()
               .HasOne<Item>(x => x.Item)
               .WithMany(y => y.Reservations)
               .HasForeignKey(z => z.ItemId);

            //////////////////////////////////////// ItemReservation one to one SerialItem
            builder.Entity<ItemReservation>()
               .HasOne<SerialItem>(x => x.SerialItem)
               .WithOne(y => y.Reservation)
               .HasForeignKey<SerialItem>(z => z.ItemReservationId);

            //////////////////////////////////////// x many to one y


            //////////////////////////////////////// x many to one y


            //////////////////////////////////////// x many to one y
        }
    }
}
