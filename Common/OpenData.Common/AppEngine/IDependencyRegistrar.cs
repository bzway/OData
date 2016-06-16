
using Autofac;

namespace OpenData.Common.AppEngine
{
    public interface IDependencyRegistrar
    {
        void Register(IContainerManager containerManager, ITypeFinder typeFinder);

        int Order { get; }
    }
}
