using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenData.Framework.WebApp.Startup))]
namespace OpenData.Framework.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<FrontPageMiddleware>();
            ConfigureAuth(app);
        }
    }
}