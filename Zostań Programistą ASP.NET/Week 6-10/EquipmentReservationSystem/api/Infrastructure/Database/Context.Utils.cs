using Domain.Interfaces.Models;
using Domain.Models;
using Infrastructure.Database.ModelConfig;
using Infrastructure.Database.ModelConfig.Business;
using Infrastructure.Database.ModelConfig.Business.MiddleTabs;
using Infrastructure.Database.ModelConfig.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Database
{
    public partial class Context
    {
        public IHttpContextAccessor _httpContextAccessor { get; set; }
        public bool SystemHostedServiceMode { get; set; } = false;

        public Context(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Waiting for Microsoft.Data.SqlClient 5.2.0 who fix SqlGuidCaster with GetAssemblies()
            
            var type = typeof(IDatabaseConfig);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            foreach (var modelConfig in types)
            {
                var configModel = Activator.CreateInstance(modelConfig);
                (Activator.CreateInstance(modelConfig) as IDatabaseConfig)?.Config(builder);
            }
            

           // ManulaFixDBModelConfig(builder);
        }

        private void ManulaFixDBModelConfig(ModelBuilder builder)
        {
            (new UserDataConfig()).Config(builder);
            (new ReservationConfig()).Config(builder);
            (new ItemInstanceConfig()).Config(builder);
            (new ItemConfig()).Config(builder);
            (new DepartmentConfig()).Config(builder);
            (new ItemToDepartmentConfig()).Config(builder);
            (new UserToDepartmentConfig()).Config(builder);

            (new RefreshTokenConfig()).Config(builder);
            (new DataFileConfig()).Config(builder);
        }

        public override int SaveChanges()
        {
            EntityAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EntityAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void EntityAudit()
        {
            var user = GetContextUser();
            var changeTrackerList = ChangeTracker.Entries<IAuditableEntity>();

            if (user == null && changeTrackerList.Any() && !SystemHostedServiceMode) throw new UserUnrecognizedException();

            foreach (var entity in changeTrackerList)
            {
                entity.Entity.UpdateBy = user?.Id ?? Guid.Empty;
                entity.Entity.UpdateTime = DateTime.Now;

                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreateBy = user.Id;
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

        public UserData GetContextUser()
        {
            var claimsIdentity = _httpContextAccessor?.HttpContext?.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == "XID")?.Value;

                if (userIdClaim != null)
                {
                    return Users.FirstOrDefault(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete && x.Id.ToString() == userIdClaim);
                }
            }
            return null;
        }
    }
}
