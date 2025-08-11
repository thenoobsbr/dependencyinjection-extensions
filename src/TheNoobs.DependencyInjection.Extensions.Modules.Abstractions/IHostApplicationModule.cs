using Microsoft.Extensions.Hosting;

namespace TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

public interface IHostApplicationModule
{
    void Setup(IHostApplicationBuilder builder);
}
