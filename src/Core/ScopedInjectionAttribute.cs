using System;

namespace TRDependencyInjection.Core
{
    public class ScopedInjectionAttribute : InjectionAttribute
    {
        public ScopedInjectionAttribute(params Type[] interfaceTypes) : base(interfaceTypes)
        {
        }
    }
}