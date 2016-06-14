using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenData.Site.FrontPage.Startup))]
namespace OpenData.Site.FrontPage
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