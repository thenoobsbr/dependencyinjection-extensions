using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TRDependencyInjection.Core;
using Xunit;

namespace TRDependencyInjection.Extensions.Tests
{
    public class DependencyInjectionExtensionsTests
    {
        [Fact]
        public void GivenDependencyInjectionWhenAddInjectionsShouldSetLifetimeCorrectly()
        {
            var servicesMock = new Mock<IServiceCollection>();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(ITransient)
                              && sd.ImplementationType == typeof(Transient)
                              && sd.Lifetime == ServiceLifetime.Transient)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(IScoped)
                              && sd.ImplementationType == typeof(Scoped)
                              && sd.Lifetime == ServiceLifetime.Scoped)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(ISingleton)
                              && sd.ImplementationType == typeof(Singleton)
                              && sd.Lifetime == ServiceLifetime.Singleton)))
                .Verifiable();
            
            servicesMock.Object.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);
            
            servicesMock.VerifyAll();
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsWhenAddShouldRegisterTransientTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var transient = provider.GetService<ITransient>();

            transient.Should().NotBeNull();
            transient.Should().BeOfType<Transient>();
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsWhenAddShouldRegisterScopedTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var transient = provider.GetService<IScoped>();

            transient.Should().NotBeNull();
            transient.Should().BeOfType<Scoped>();
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsWhenAddShouldRegisterSingletonTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var transient = provider.GetService<ISingleton>();

            transient.Should().NotBeNull();
            transient.Should().BeOfType<Singleton>();
        }

        private interface ITransient
        {            
        }
        private class Transient : ITransientInjection<ITransient>, ITransient
        {
        }
        
        private interface IScoped
        {            
        }
        private class Scoped : IScopedInjection<IScoped>, IScoped
        {
        }
        
        private interface ISingleton
        {            
        }
        private class Singleton : ISingletonInjection<ISingleton>, ISingleton
        {
        }
    }
}
