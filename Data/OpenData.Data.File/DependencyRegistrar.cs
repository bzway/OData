using OpenData.Common.AppEngine;
using OpenData.Data.Core;
namespace OpenData.Data.Default
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterType<IDatabase, FileDatabase>("Default");
        }
        public int Order
        {
            get { return 10; }
        }
    }
}
