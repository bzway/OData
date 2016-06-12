using Microsoft.Owin.Security.Provider;
using System;
namespace Microsoft.Owin.Security.WeChat
{
	public class WeChatReturnEndpointContext : ReturnEndpointContext
	{
		public WeChatReturnEndpointContext(IOwinContext context, AuthenticationTicket ticket) : base(context, ticket)
		{
		}
	}
}
