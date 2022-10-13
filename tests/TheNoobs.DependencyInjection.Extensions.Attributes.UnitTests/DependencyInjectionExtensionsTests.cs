﻿using System;
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
        public void GivenDependencyInjectionExtensionsShouldAddFactoryTypes()
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
        
        [TransientInjection(typeof(ITransient))]
        private class Transient : ITransient
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
        [ScopedInjection(typeof(IScoped), typeof(IScoped2))]
        private class Scoped : IScoped, IScoped2
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
        [SingletonInjection(typeof(ISingleton))]
        private class Singleton : ISingleton
        {
        }

        [ScopedInjection]
        private class ScopedAllAlone
        {
        }
    }
}