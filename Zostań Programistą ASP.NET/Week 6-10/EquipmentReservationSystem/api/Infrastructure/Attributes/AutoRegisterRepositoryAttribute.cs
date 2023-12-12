using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterRepositoryAttribute : Attribute
    {
        public Type InterfaceType { get; }
        public DependencyInjectionRepositoryType RegisterType { get; }

        public AutoRegisterRepositoryAttribute(Type serviceInterface, DependencyInjectionRepositoryType registerType)
        {
            InterfaceType = serviceInterface;
            RegisterType = registerType;
        }

        public virtual void OnRegister(IServiceCollection services, Type serviceType)
        {
            switch (RegisterType)
            {
                case DependencyInjectionRepositoryType.TransientRepository:
                    services.AddTransient(InterfaceType, serviceType);
                    break;
                case DependencyInjectionRepositoryType.ScopedRepository:
                    services.AddScoped(InterfaceType, serviceType);
                    break;
                case DependencyInjectionRepositoryType.SingletonRepository:
                    services.AddSingleton(InterfaceType, serviceType);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public enum DependencyInjectionRepositoryType
        {
            TransientRepository,
            ScopedRepository,
            SingletonRepository
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterTransientRepositoryAttribute : AutoRegisterRepositoryAttribute
    {
        public AutoRegisterTransientRepositoryAttribute(Type serviceInterface) : base(serviceInterface, DependencyInjectionRepositoryType.TransientRepository)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterScopedRepositoryAttribute : AutoRegisterRepositoryAttribute
    {
        public AutoRegisterScopedRepositoryAttribute(Type serviceInterface) : base(serviceInterface, DependencyInjectionRepositoryType.ScopedRepository)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterSingletonRepositoryAttribute : AutoRegisterRepositoryAttribute
    {
        public AutoRegisterSingletonRepositoryAttribute(Type serviceInterface) : base(serviceInterface, DependencyInjectionRepositoryType.SingletonRepository)
        {
        }
    }
}
