
using OpenData.Common.AppEngine;
using System.Configuration;


namespace OpenData.Log
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private string ConnectionString;

        public void Register(IContainerManager builder, ITypeFinder typeFinder)
        {
            try
            {
                this.ConnectionString = ConfigurationManager.ConnectionStrings["ApplicationRepository"].ConnectionString;
                Logger.Register(ConnectionString);
            }
            catch
            {
            }
        }

        public int Order
        {
            get { return 1000; }
        }
    }
}