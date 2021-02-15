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
                services.Add(ServiceDescriptor.Describe(
                    implementationType,
                    implementationType,
                    GetServiceLifetime(attribute)));
                
                services.Add(ServiceDescriptor.Describe(implementationType, implementationType, attribute.GetServiceLifetime()));
                foreach (var interfaceType in attribute.InterfaceTypes)
                {
                    services.Add(ServiceDescriptor.Describe(
                        interfaceType,
                        provider => provider.GetService(implementationType),
                        GetServiceLifetime(attribute)));
                }
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
