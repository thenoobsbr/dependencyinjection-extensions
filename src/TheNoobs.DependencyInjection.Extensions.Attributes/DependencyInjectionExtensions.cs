using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions;

namespace TheNoobs.DependencyInjection.Extensions.Attributes
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInjections(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddInjectionAttributes(assemblies);
        }

        private static void AddInjectionAttributes(this IServiceCollection services, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetTypes())
                .Where(HasInjectionAttributes).ToList();
            foreach (var implementationType in types)
            {
                RegisterServiceToInterfaces(services, implementationType);
            }

            bool HasInjectionAttributes(Type type)
            {
                return type.GetCustomAttributes<InjectionAttribute>().Any();
            }
        }

        private static void RegisterServiceToInterfaces(IServiceCollection services, Type implementationType)
        {
            var attributes = implementationType.GetCustomAttributes<InjectionAttribute>().ToList();

            foreach (var attribute in attributes)
            {
                Register(attribute);
            }

            void Register(InjectionAttribute attribute)
            {
                if (attribute.RegisteredType is null)
                {
                    services.Add(ServiceDescriptor.Describe(implementationType, implementationType,
                        attribute.GetServiceLifetime()));
                }

                foreach (var interfaceType in attribute.InterfaceTypes)
                {
                    services.Add(ServiceDescriptor.Describe(
                        interfaceType,
                        sp => CreateService(sp, attribute),
                        GetServiceLifetime(attribute)));
                }
            }
            
            object CreateService(IServiceProvider provider, InjectionAttribute attribute)
            {
                return provider.GetRequiredService(attribute.RegisteredType ?? implementationType);
            }
        }

        private static ServiceLifetime GetServiceLifetime(this InjectionAttribute attribute)
        {
            return attribute switch
            {
                ScopedInjectionAttribute _ => ServiceLifetime.Scoped,
                SingletonInjectionAttribute _ => ServiceLifetime.Singleton,
                _ => ServiceLifetime.Transient
            };
        }
    }
}
