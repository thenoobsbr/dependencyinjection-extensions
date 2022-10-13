namespace TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public abstract class InjectionAttribute : Attribute
    {
        protected InjectionAttribute(Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
            RegisteredType = null;
        }
        
        protected InjectionAttribute(Type[] interfaceTypes, Type registeredType) : this(interfaceTypes)
        {
            RegisteredType = registeredType ?? throw new ArgumentNullException(nameof(registeredType));
        }
        
        public Type[] InterfaceTypes { get; }
        public Type? RegisteredType { get; }
    }
}
