using Application.Attributes;
using Application.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Application.Utils
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            AddService(services);
            AddHostedService(services);
            return services;
        }

        private static void AddService(IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(s => Attribute.IsDefined(s, typeof(AutoRegisterServiceAttribute)));

            foreach (var type in types)
            {
                type.GetCustomAttributes<AutoRegisterServiceAttribute>(true).First().OnRegister(services, type);
            }
        }

        private static void AddHostedService(IServiceCollection services)
        {
            services.AddHostedService<FileHostedService>();
        }
    }

}
