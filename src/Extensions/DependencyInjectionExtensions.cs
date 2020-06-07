using System;
using System.Collections.Generic;
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
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<InjectionAttribute>();
                if (attribute != null)
                {
                    services.Add(ServiceDescriptor.Describe(attribute.InterfaceType, type, attribute.GetServiceLifetime()));
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
