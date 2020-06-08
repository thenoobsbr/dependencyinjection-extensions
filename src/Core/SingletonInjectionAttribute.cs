using System;

namespace TRDependencyInjection.Core
{
    public class SingletonInjectionAttribute : InjectionAttribute
    {
        public SingletonInjectionAttribute(params Type[] interfaceTypes) : base(interfaceTypes)
        {
        }
    }
}