using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheNoobs.DependencyInjection.Extensions.Modules.Abstractions
{
    public interface IServiceModuleSetup
    {
        void Setup(IServiceCollection services, IConfiguration configuration);  
    }
}
