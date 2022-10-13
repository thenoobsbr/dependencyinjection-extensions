namespace TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions
{
    public class SingletonInjectionAttribute : InjectionAttribute
    {
        public SingletonInjectionAttribute() : base(new Type[]{})
        {
        }
        
        public SingletonInjectionAttribute(Type interfaceType) : base(new [] { interfaceType })
        {
        }
        
        public SingletonInjectionAttribute(Type interfaceType1, Type interfaceType2) : base(new [] { interfaceType1, interfaceType2 })
        {
        }
        
        public SingletonInjectionAttribute(Type interfaceType1, Type interfaceType2, Type interfaceType3) : base(new [] { interfaceType1, interfaceType2, interfaceType3 })
        {
        }

        public SingletonInjectionAttribute(Type[] interfaceTypes, Type registeredType) : base(interfaceTypes, registeredType)
        {
        }
    }
}
