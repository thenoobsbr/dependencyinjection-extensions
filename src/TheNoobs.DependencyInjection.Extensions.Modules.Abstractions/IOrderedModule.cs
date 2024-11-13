namespace TheNoobs.DependencyInjection.Extensions.Modules.Abstractions;

public interface IOrderedModule
{
    int Order { get; }
}
