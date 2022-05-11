using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TRDependencyInjection.Core
{
    public interface IServiceModuleSetup
    {
        void Setup(IServiceCollection services, IConfiguration configuration);  
    }
}