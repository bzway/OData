using OpenData.AppEngine.Dependency;
using OpenData.Caching;
using OpenData.Data.Core;
using OpenData.Message;
using System.Web;
namespace OpenData.Data.SQLServer
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public void Register(ContainerManager containerManager, AppEngine.ITypeFinder typeFinder)
        {
            containerManager.AddComponent<IDatabase, SQLServerDatabase>("SQLServer");
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
