using Application.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Utils
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(s => Attribute.IsDefined(s, typeof(AutoRegisterServiceAttribute)));

            foreach (var type in types)
            {
                type.GetCustomAttributes<AutoRegisterServiceAttribute>(true).First().OnRegister(services, type);
            }

            return services;
        }
    }

}
