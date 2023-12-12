using Domain.Interfaces.Models;
using Infrastructure.Database.ModelConfig.Business;
using Infrastructure.Database.ModelConfig.Business.MiddleTabs;
using Infrastructure.Database.ModelConfig.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public partial class Context
    {
        public IHttpContextAccessor _httpContextAccessor { get; set; }

        public Context(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Waiting for Microsoft.Data.SqlClient 5.2.0 who fix SqlGuidCaster with GetAssemblies()
            /*
            var type = typeof(IDatabaseConfig);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !type.IsInterface && !type.IsAbstract);

            foreach (var modelConfig in types)
            {
                var configModel = Activator.CreateInstance(modelConfig);
                (Activator.CreateInstance(modelConfig) as IDatabaseConfig)?.Config(builder);
            }
            */

            AlfaFixManulaConfig(builder);
        }

        private void AlfaFixManulaConfig(ModelBuilder builder)
        {
            (new UserDataConfig()).Config(builder);
            (new ReservationConfig()).Config(builder);
            (new ItemInstanceConfig()).Config(builder);
            (new ItemConfig()).Config(builder);
            (new DepartmentConfig()).Config(builder);
            (new ItemToDepartmentConfig()).Config(builder);
            (new UserToDepartmentConfig()).Config(builder);

            (new DataFileConfig()).Config(builder);
            (new DictionaryConfig()).Config(builder);
        }

        public override int SaveChanges()
        {
            EntityAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EntityAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void EntityAudit()
        {
            foreach (var entity in ChangeTracker.Entries<IAuditableEntity>())
            {
                //entity.Entity.UpdateBy = 
                entity.Entity.UpdateTime = DateTime.Now;

                switch (entity.State)
                {
                    case EntityState.Added:
                        //entity.Entity.CreateBy = new 
                        entity.Entity.CreateTime = DateTime.Now;
                        entity.Entity.EntityStatus ??= Domain.Utils.EntityStatus.Use; 
                        break;
                    case EntityState.Modified:
                        break;
                    case EntityState.Deleted:
                        entity.Entity.EntityStatus = Domain.Utils.EntityStatus.Delete;
                        entity.State = EntityState.Modified;
                        break;
                }
            }
        }
    }
}
