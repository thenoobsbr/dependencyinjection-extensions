using System;

namespace TRDependencyInjection.Core
{
    public class TransientInjectionAttribute : InjectionAttribute
    {
        public TransientInjectionAttribute(Type interfaceType) : base(interfaceType)
        {
        }
    }
}
