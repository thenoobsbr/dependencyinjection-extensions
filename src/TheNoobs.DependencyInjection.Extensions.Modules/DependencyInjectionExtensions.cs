using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

namespace TheNoobs.DependencyInjection.Extensions.Modules;

public static class DependencyInjectionExtensions
{
    public static void AddInjections(this IHostApplicationBuilder builder, params Assembly[] assemblies)
    {
        foreach (var module in GetModuleSetups<IHostApplicationModule>(assemblies))
        {
            module.Setup(builder);
        }
        
        builder.Services.AddInjectionModules(builder.Configuration, assemblies);
    }
        
    public static void UseInjections(this IApplicationBuilder applicationBuilder, params Assembly[] assemblies)
    {
        foreach (var module in GetModuleSetups<IApplicationModuleSetup>(assemblies))
        {
            module.Setup(applicationBuilder);
        }
    }
    
    private static void AddInjectionModules(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        foreach (var module in GetModuleSetups<IServiceModuleSetup>(assemblies))
        {
            module.Setup(services, configuration);
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
