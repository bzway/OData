using OpenData.Common.AppEngine;
using OpenData.Common.Caching;
using OpenData.Data.Core;
using OpenData.Message;
using System.Web;
namespace OpenData.Data.SQLServer
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterType<IDatabase, SQLServerDatabase>("SQLServer");
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
