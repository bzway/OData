using OpenData.Common.AppEngine;
using OpenData.Caching;
using OpenData.Data.Core;
using OpenData.Message;
using System.Web;
namespace OpenData.Data.Mongo
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterType<IDatabase, MongoDatabase>("MongoDB");
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
