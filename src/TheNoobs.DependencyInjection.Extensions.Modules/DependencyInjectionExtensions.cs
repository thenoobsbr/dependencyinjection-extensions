using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

namespace TheNoobs.DependencyInjection.Extensions.Modules;

public static class DependencyInjectionExtensions
{
    public static void AddInjections(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddInjectionModules(configuration, assemblies);
    }
        
    private static void AddInjectionModules(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
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
            return typeof(T).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false };
        }
    }
}
