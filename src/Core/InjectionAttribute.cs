using System;

namespace TRDependencyInjection.Core
{
    public abstract class InjectionAttribute : Attribute
    {
        protected InjectionAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
        
        public Type InterfaceType { get; }
    }
}