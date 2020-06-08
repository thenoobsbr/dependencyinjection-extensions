using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TRDependencyInjection.Core;

namespace TRDependencyInjection.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInjections(this IServiceCollection services, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetTypes())
                .Where(x => x.GetCustomAttribute<InjectionAttribute>() != null).ToList();
            foreach (var implementationType in types)
            {
                var attribute = implementationType.GetCustomAttribute<InjectionAttribute>();
                var mainInterfaceType = attribute.InterfaceTypes.First();
                
                services.Add(ServiceDescriptor.Describe(mainInterfaceType, implementationType, attribute.GetServiceLifetime()));
                foreach (var interfaceType in attribute.InterfaceTypes.Skip(1))
                {
                    services.Add(ServiceDescriptor.Describe(
                        interfaceType,
                        provider => provider.GetService(mainInterfaceType),
                        GetServiceLifetime(attribute)));
                }
            }
        }

        private static void ValidateTypeToInterface(Type implementationType, Type interfaceType)
        {
            if (!interfaceType.IsAssignableFrom(implementationType))
            {
                throw new InvalidCastException($"The type {implementationType.FullName} is not assignable to {interfaceType.FullName}.");
            }
        }

        private static ServiceLifetime GetServiceLifetime(this InjectionAttribute attribute)
        {
            switch (attribute)
            {
                case ScopedInjectionAttribute _: return ServiceLifetime.Scoped;
                case SingletonInjectionAttribute _: return ServiceLifetime.Singleton;
                default: return ServiceLifetime.Transient;
            }
        }
    }
}
