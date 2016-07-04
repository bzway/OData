using System.Threading.Tasks;
namespace OpenData.Security.WeChat
{
    public interface IWeChatAuthenticationProvider
	{
		Task Authenticated(WeChatAuthenticatedContext context);
		Task ReturnEndpoint(WeChatReturnEndpointContext context);
	}
}
