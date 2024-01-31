using Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Utils
{
    public static class RepositoriesDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(s => Attribute.IsDefined(s, typeof(AutoRegisterRepositoryAttribute)));

            foreach (var type in types)
            {
                type.GetCustomAttributes<AutoRegisterRepositoryAttribute>(true).First().OnRegister(services, type);
            }

            return services;
        }
    }

}
