using Microsoft.AspNetCore.Builder;

namespace TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

public interface IApplicationModuleSetup
{
        
    void Setup(IApplicationBuilder appBuilder);
}
