using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;
using Xunit;

namespace TheNoobs.DependencyInjection.Extensions.Modules.UnitTests
{
    public class DependencyInjectionExtensionsTests
    {
        [Fact]
        public void GivenDependencyInjectionExtensionsWhenAddModuleShouldSetup()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(configuration, typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var testClass = provider.GetService<ModuleTestClass>();

            testClass.Should().NotBeNull();
            testClass.Should().BeOfType<ModuleTestClass>();
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsWhenUseModuleShouldSetup()
        {
            var servicesMock = new Mock<IServiceProvider>();
            var appBuilder = new Mock<IApplicationBuilder>();
            
            appBuilder.Setup(x => x.ApplicationServices).Returns(servicesMock.Object)
                .Verifiable();
            servicesMock.Setup(x => x.GetService(typeof(ModuleTestClass)))
                .Returns(new ModuleTestClass())
                .Verifiable();

            appBuilder.Object.UseInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            appBuilder.VerifyAll();
            servicesMock.VerifyAll();
        }

        private class ModuleTestClass
        {
        }

        private class ServiceModuleSetup : IServiceModuleSetup
        {
            public void Setup(IServiceCollection services, IConfiguration configuration)
            {
                services.AddScoped<ModuleTestClass>();
            }
        }
        
        private class AppBuilderApplicationModuleSetup : IApplicationModuleSetup
        {
            public void Setup(IApplicationBuilder applicationBuilder)
            {
                var _ = applicationBuilder.ApplicationServices.GetRequiredService<ModuleTestClass>();
            }
        }

        private class OrderClassTest
        {
        }

        private class Module1Setup : IServiceModuleSetup, IOrderedModule
        {
            public void Setup(IServiceCollection services, IConfiguration configuration)
            {
                services.Any(x => x.ServiceType == typeof(OrderClassTest)).Should().BeTrue();
            }

            public int Order => 2;
        }
        
        private class Module2Setup : IServiceModuleSetup, IOrderedModule
        {
            public void Setup(IServiceCollection services, IConfiguration configuration)
            {
                services.Any(x => x.ServiceType == typeof(OrderClassTest)).Should().BeFalse();
                services.AddScoped<OrderClassTest>();
            }

            public int Order => 1;
        }
    }
}
