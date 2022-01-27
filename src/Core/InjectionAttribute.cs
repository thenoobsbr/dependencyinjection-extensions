using System;

namespace TRDependencyInjection.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public abstract class InjectionAttribute : Attribute
    {
        protected InjectionAttribute(Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
        }
        
        protected InjectionAttribute(Type[] interfaceTypes, Type registeredType) : this(interfaceTypes)
        {
            RegisteredType = registeredType ?? throw new ArgumentNullException(nameof(registeredType));
        }
        
        public Type[] InterfaceTypes { get; }
        public Type RegisteredType { get; }
    }
}