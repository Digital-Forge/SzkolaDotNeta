using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.Services;
using ResourceManagementSystem.Application.ViewModel.Init;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services Dependency Injection
            services.AddTransient<IInitService, InitService>();

            // FluentValidation Dependency Injection
            services.AddTransient<IValidator<AdminAccountVM>, AdminAccountValidation>();

            return services;
        }
    }
}
