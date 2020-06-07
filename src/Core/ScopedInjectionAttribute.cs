using System;

namespace TRDependencyInjection.Core
{
    public class ScopedInjectionAttribute : InjectionAttribute
    {
        public ScopedInjectionAttribute(Type interfaceType) : base(interfaceType)
        {
        }
    }
}