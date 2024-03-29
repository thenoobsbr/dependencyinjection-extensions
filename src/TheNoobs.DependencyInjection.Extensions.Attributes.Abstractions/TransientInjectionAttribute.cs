﻿namespace TheNoobs.DependencyInjection.Extensions.Attributes.Abstractions
{
    public class TransientInjectionAttribute : InjectionAttribute
    {
        public TransientInjectionAttribute() : base(Array.Empty<Type>())
        {
        }
        
        public TransientInjectionAttribute(Type interfaceType) : base(new [] { interfaceType })
        {
        }
        
        public TransientInjectionAttribute(Type interfaceType1, Type interfaceType2) : base(new [] { interfaceType1, interfaceType2 })
        {
        }
        
        public TransientInjectionAttribute(Type interfaceType1, Type interfaceType2, Type interfaceType3) : base(new [] { interfaceType1, interfaceType2, interfaceType3 })
        {
        }
    }
}
