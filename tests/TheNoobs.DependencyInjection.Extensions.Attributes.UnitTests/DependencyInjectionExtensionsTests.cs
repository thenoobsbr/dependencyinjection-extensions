using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions;
using Xunit;

namespace TheNoobs.DependencyInjection.Extensions.Attributes.UnitTests
{
    public class DependencyInjectionExtensionsTests
    {
        [Fact]
        public void GivenDependencyInjectionWhenAddInjectionsShouldSetLifetimeCorrectly()
        {
            var servicesMock = new Mock<IServiceCollection>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(Transient)))
                .Returns(new Transient())
                .Verifiable();
            serviceProviderMock.Setup(x => x.GetService(typeof(Scoped)))
                .Returns(new Scoped())
                .Verifiable();
            serviceProviderMock.Setup(x => x.GetService(typeof(Singleton)))
                .Returns(new Singleton())
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(Transient)
                              && sd.ImplementationType == typeof(Transient)
                              && sd.Lifetime == ServiceLifetime.Transient)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(ITransient)
                              && sd.ImplementationFactory(serviceProviderMock.Object) is ITransient
                              && sd.Lifetime == ServiceLifetime.Transient)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(Scoped)
                              && sd.ImplementationType == typeof(Scoped)
                              && sd.Lifetime == ServiceLifetime.Scoped)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(IScoped)
                              && sd.ImplementationFactory(serviceProviderMock.Object) is IScoped
                              && sd.Lifetime == ServiceLifetime.Scoped)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(Singleton)
                              && sd.ImplementationType == typeof(Singleton)
                              && sd.Lifetime == ServiceLifetime.Singleton)))
                .Verifiable();
            servicesMock.Setup(x => 
                    x.Add(It.Is<ServiceDescriptor>(
                        sd => sd.ServiceType == typeof(ISingleton)
                              && sd.ImplementationFactory(serviceProviderMock.Object) is ISingleton
                              && sd.Lifetime == ServiceLifetime.Singleton)))
                .Verifiable();
            
            servicesMock.Object.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);
            
            servicesMock.VerifyAll();
            serviceProviderMock.VerifyAll();
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
            var scoped = provider.GetService<IScoped>();

            scoped.Should().NotBeNull();
            scoped.Should().BeOfType<Scoped>();
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsWhenAddShouldRegisterSingletonTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var singleton = provider.GetService<ISingleton>();

            singleton.Should().NotBeNull();
            singleton.Should().BeOfType<Singleton>();
        }

        [Fact]
        public void GivenDependencyInjectionExtensionsShouldAddMultipleTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var scoped = provider.GetService<IScoped>();
            var scoped2 = provider.GetService<IScoped2>();

            scoped.Should().NotBeNull();
            scoped.Should().Be(scoped2);
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsShouldAddScopedFactoryTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var scoped = provider.GetService<IScopedFactory>();
            var scoped2 = provider.GetService<IScopedFactory2>();

            scoped.Should().NotBeNull();
            scoped.Should().Be(scoped2);
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsShouldAddSingletonFactoryTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var factory = provider.GetService<ISingletonFactory>();
            var factory2 = provider.GetService<ISingletonFactory2>();

            factory.Should().NotBeNull();
            factory.Should().Be(factory2);
        }
        
        [Fact]
        public void GivenDependencyInjectionExtensionsShouldAddImplementationTypeAsInterface()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddInjections(typeof(DependencyInjectionExtensionsTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var scoped = provider.GetService<ScopedAllAlone>();

            scoped.Should().NotBeNull();
        }

        private interface ITransient
        {            
        }
        
        private interface ITransient2
        {            
        }
        
        private interface ITransient3
        {            
        }
        
        [TransientInjection]
        [TransientInjection(typeof(ITransient))]
        [TransientInjection(typeof(ITransient), typeof(ITransient2))]
        [TransientInjection(typeof(ITransient), typeof(ITransient2), typeof(ITransient3))]
        private class Transient : ITransient, ITransient2, ITransient3
        {
        }
        
        private interface IScoped
        {            
        }
        private interface IScopedFactory
        {
        }
        private interface IScopedFactory2
        {
        }
        private interface IScoped2
        {            
        }
        
        private interface IScoped3
        {
        }
        
        [ScopedInjection]
        [ScopedInjection(typeof(IScoped))]
        [ScopedInjection(typeof(IScoped), typeof(IScoped2))]
        [ScopedInjection(typeof(IScoped), typeof(IScoped2), typeof(IScoped3))]
        private class Scoped : IScoped, IScoped2, IScoped3
        {
        }

        [ScopedInjection(typeof(IScopedFactory))]
        [ScopedInjection(new [] { typeof(IScopedFactory2) }, typeof(IScopedFactory))]
        private class ScopedFactory : IScopedFactory, IScopedFactory2
        {
        }
        
        private interface ISingleton
        {            
        }
        
        private interface ISingleton2
        {            
        }
        
        private interface ISingleton3
        {            
        }
        
        [SingletonInjection]
        [SingletonInjection(typeof(ISingleton))]
        [SingletonInjection(typeof(ISingleton), typeof(ISingleton2))]
        [SingletonInjection(typeof(ISingleton), typeof(ISingleton2), typeof(ISingleton3))]
        private class Singleton : ISingleton
        {
        }
        
        private interface ISingletonFactory
        {
        }
        
        private interface ISingletonFactory2
        {
        }

        [SingletonInjection(typeof(ISingletonFactory))]
        [SingletonInjection(new []{ typeof(ISingletonFactory2) }, typeof(ISingletonFactory))]
        private class SingletonFactory : ISingletonFactory, ISingletonFactory2
        {
        }

        [ScopedInjection]
        private class ScopedAllAlone
        {
        }
    }
}
