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
                .Where(IsInjectionImplementation).ToList();
            foreach (var type in types)
            {
                var interfaces = type.GetInjectionInterfaces().ToList();
                foreach (var i in interfaces)
                {
                    services.Add(ServiceDescriptor.Describe(i.GenericTypeArguments.First(), type, i.GetServiceLifetime()));
                }
            }
        }

        private static ServiceLifetime GetServiceLifetime(this Type type)
        {
            if (type.GetGenericTypeDefinition() == typeof(ISingletonInjection<>))
            {
                return ServiceLifetime.Singleton;
            }
            if (type.GetGenericTypeDefinition() == typeof(IScopedInjection<>))
            {
                return ServiceLifetime.Scoped;
            }
            return ServiceLifetime.Transient;
        }

        private static bool IsImplementation(this Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }
        private static bool IsInjectionImplementation(this Type type)
        {
            return type.IsImplementation() && type.GetInjectionInterfaces().Any();
        }
        private static IEnumerable<Type> GetInjectionInterfaces(this Type type)
        {
            return type.GetInterfaces()
                .Where(i =>
                    i.IsGenericType
                    && typeof(IInjection).IsAssignableFrom(i.GetGenericTypeDefinition())).ToList();
        }


    }
}
