using Microsoft.Extensions.DependencyInjection;

namespace Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterServiceAttribute : Attribute
    {
        public Type InterfaceType { get; }
        public DependencyInjectionServiceType RegisterType { get; }

        public AutoRegisterServiceAttribute(Type serviceInterface, DependencyInjectionServiceType registerType = DependencyInjectionServiceType.TransientService)
        {
            InterfaceType = serviceInterface;
            RegisterType = registerType;
        }

        public virtual void OnRegister(IServiceCollection services, Type serviceType) 
        {
            switch (RegisterType)
            {
                case DependencyInjectionServiceType.TransientService:
                    services.AddTransient(InterfaceType, serviceType);
                    break;
                case DependencyInjectionServiceType.ScopedService:
                    services.AddScoped(InterfaceType, serviceType);
                    break;
                case DependencyInjectionServiceType.SingletonService:
                    services.AddSingleton(InterfaceType, serviceType);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public enum DependencyInjectionServiceType
        {
            TransientService,
            ScopedService,
            SingletonService
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterTransientServiceAttribute : AutoRegisterServiceAttribute
    {
        public AutoRegisterTransientServiceAttribute(Type serviceInterface) : base(serviceInterface, DependencyInjectionServiceType.TransientService)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterScopedServiceAttribute : AutoRegisterServiceAttribute
    {
        public AutoRegisterScopedServiceAttribute(Type serviceInterface) : base(serviceInterface, DependencyInjectionServiceType.ScopedService)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterSingletonServiceAttribute : AutoRegisterServiceAttribute
    {
        public AutoRegisterSingletonServiceAttribute(Type serviceInterface) : base(serviceInterface, DependencyInjectionServiceType.SingletonService)
        {
        }
    }
}
