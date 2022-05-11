using Microsoft.AspNetCore.Builder;

namespace TRDependencyInjection.Core
{
    public interface IApplicationModuleSetup
    {
        void Setup(IApplicationBuilder appBuilder);
    }
}