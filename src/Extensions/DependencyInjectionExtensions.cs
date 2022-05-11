using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TRDependencyInjection.Core;

namespace TRDependencyInjection.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInjections(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddInjectionAttributes(assemblies);
        }
        
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddInjectionAttributes(assemblies);
            services.AddInjectionModules(configuration, assemblies);
        }
        
        public static void AddInjectionModules(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            foreach (var serviceSetup in GetModuleSetups<IServiceModuleSetup>(assemblies))
            {
                serviceSetup.Setup(services, configuration);
            }
        }
        
        public static void UseInjections(this IApplicationBuilder applicationBuilder, params Assembly[] assemblies)
        {
            foreach (var serviceSetup in GetModuleSetups<IApplicationModuleSetup>(assemblies))
            {
                serviceSetup.Setup(applicationBuilder);
            }
        }

        public static void AddInjectionAttributes(this IServiceCollection services, params Assembly[] assemblies)
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
        
        private static ICollection<T> GetModuleSetups<T>(Assembly[] assemblies)
        {
            return assemblies
                .SelectMany(a => a.GetTypes())
                .Where(IsModuleServiceSetup)
                .Select(Activator.CreateInstance)
                .Cast<T>()
                .OrderBy(x => x is IOrderedModule orderedModule ? orderedModule.Order : int.MaxValue)
                .ToList();
            
            bool IsModuleServiceSetup(Type type)
            {
                return typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract;
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
                if (attribute.RegisteredType is null)
                {
                    return provider.GetService(implementationType);
                }

                return provider.GetService(attribute.RegisteredType);
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
