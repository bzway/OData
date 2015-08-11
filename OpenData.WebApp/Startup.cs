using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenData.WebApp.Startup))]
namespace OpenData.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
