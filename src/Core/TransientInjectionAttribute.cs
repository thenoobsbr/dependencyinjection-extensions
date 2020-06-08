using System;

namespace TRDependencyInjection.Core
{
    public class TransientInjectionAttribute : InjectionAttribute
    {
        public TransientInjectionAttribute(params Type[] interfaceTypes) : base(interfaceTypes)
        {
        }
    }
}
