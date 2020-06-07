using System;

namespace TRDependencyInjection.Core
{
    public class SingletonInjectionAttribute : InjectionAttribute
    {
        public SingletonInjectionAttribute(Type interfaceType) : base(interfaceType)
        {
        }
    }
}