using System;

namespace TRDependencyInjection.Core
{
    public abstract class InjectionAttribute : Attribute
    {
        protected InjectionAttribute(Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
        }
        
        public Type[] InterfaceTypes { get; }
    }
}