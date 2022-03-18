using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.Services;
using ResourceManagementSystem.Application.ViewModel.Init;
using ResourceManagementSystem.Application.ViewModel.Users;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ResourceManagementSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services Dependency Injection
            services.AddTransient<IInitService, InitService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUsersModerateService, UsersModerateService>();
            services.AddTransient<IDepartmentModerateService, DepartmentModerateService>();
            services.AddTransient<IItemModerateService, ItemModerateService>();
            services.AddTransient<IReservationService, ReservationService>();

            // FluentValidation Dependency Injection
            services.AddTransient<IValidator<AdminAccountVM>, AdminAccountValidation>();
            services.AddTransient<IValidator<CreateUserVM>, CreateUserValidation>();
            services.AddTransient<IValidator<DetailsEditUserVM>, DetailsEditUserValidation>();

            // AutoMapper Dependency Injection
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
