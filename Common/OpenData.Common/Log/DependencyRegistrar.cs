using OpenData.AppEngine;
using OpenData.AppEngine.Dependency;
using System.Configuration;


namespace OpenData.Log
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private string ConnectionString;

        public void Register(ContainerManager builder, ITypeFinder typeFinder)
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