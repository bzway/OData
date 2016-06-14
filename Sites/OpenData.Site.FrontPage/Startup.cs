using Microsoft.Owin;
using OpenData.Framework.Common;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenData.Sites.FrontPage.Startup))]

namespace OpenData.Sites.FrontPage
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