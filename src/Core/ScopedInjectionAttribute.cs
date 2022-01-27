using System;

namespace TRDependencyInjection.Core
{
    public class ScopedInjectionAttribute : InjectionAttribute
    {
        public ScopedInjectionAttribute() : base(new Type[]{})
        {
        }
        
        public ScopedInjectionAttribute(Type interfaceType) : base(new [] { interfaceType })
        {
        }
        
        public ScopedInjectionAttribute(Type interfaceType1, Type interfaceType2) : base(new [] { interfaceType1, interfaceType2 })
        {
        }
        
        public ScopedInjectionAttribute(Type interfaceType1, Type interfaceType2, Type interfaceType3) : base(new [] { interfaceType1, interfaceType2, interfaceType3 })
        {
        }

        public ScopedInjectionAttribute(Type[] interfaceTypes, Type registeredType) : base(interfaceTypes, registeredType)
        {
        }
    }
}