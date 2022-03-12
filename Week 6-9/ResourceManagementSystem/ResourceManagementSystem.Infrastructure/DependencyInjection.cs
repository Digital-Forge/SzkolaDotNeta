using Microsoft.Extensions.DependencyInjection;
using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IAppSettingPropertyRepository, AppSettingPropertyRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IAccessConfigRepository, AccessConfigRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();

            return services;
        }
    }
}
