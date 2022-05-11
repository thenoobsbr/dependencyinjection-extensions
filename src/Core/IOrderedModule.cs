namespace TRDependencyInjection.Core
{
    public interface IOrderedModule
    {
        int Order { get; }
    }
}