namespace OpenData.AppEngine.Dependency
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerManager containerManager, ITypeFinder typeFinder);

        int Order { get; }
    }
}
