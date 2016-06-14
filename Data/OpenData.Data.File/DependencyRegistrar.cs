using OpenData.AppEngine.Dependency;
using OpenData.Data.Core;
namespace OpenData.Data.Default
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerManager containerManager, AppEngine.ITypeFinder typeFinder)
        {
            containerManager.AddComponent<IDatabase, FileDatabase>("Default");
        }
        public int Order
        {
            get { return 10; }
        }
    }
}
