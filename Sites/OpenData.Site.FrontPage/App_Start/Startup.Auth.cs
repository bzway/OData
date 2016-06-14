using Owin;
using OpenData.Security.WeChat;

namespace OpenData.Sites.FrontPage
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWeChatAuthentication(new WeChatAuthenticationOptions()
            {
                AppId = "",
                AppSecret = ""
            });
        }
    }
}