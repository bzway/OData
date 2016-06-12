using System;
using System.Threading.Tasks;
namespace Microsoft.Owin.Security.WeChat
{
	public interface IWeChatAuthenticationProvider
	{
		Task Authenticated(WeChatAuthenticatedContext context);
		Task ReturnEndpoint(WeChatReturnEndpointContext context);
	}
}
