//using Autofac;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace OpenData.Framework.Core
//{
//    public class UserSiteMiddleware
//    {
//        private static Dictionary<string, IServiceProvider> serviceProviderCache = new Dictionary<string, IServiceProvider>();
//        private static object lockObject = new object();
//        private readonly RequestDelegate next;
//        private readonly IServiceCollection gloableServices;

//        public UserSiteMiddleware(RequestDelegate next, IServiceCollection gloableServices)
//        {
//            this.next = next;
//            this.gloableServices = gloableServices;
//        }
//        public Task Invoke(HttpContext context)
//        {
//            //find tenant by host name
//            var siteService = context.RequestServices.GetService<ISiteService>();
//            if (siteService == null)
//            {
//                throw new NotSupportedException("App Service is not registered");
//            }
//            var site = siteService.FindSiteByDomain(context.Request.Host.Host);
//            //try to get private User site's ServiceProvider

//            if (serviceProviderCache.ContainsKey(site.Name))
//            {
//                context.RequestServices = serviceProviderCache[site.Name];
//                return next(context);
//            }
//            lock (lockObject)
//            {
//                if (!serviceProviderCache.ContainsKey(site.Name))
//                {

//                    var containerBuilder = new ContainerBuilder();
//                    foreach (var item in gloableServices)
//                    {
//                        containerBuilder.RegisterType(item.ImplementationType).As(item.ServiceType);
//                    }
//                    //containerBuilder.Populate(gloableServices);


//                    IServiceCollection proviteServices = new ServiceCollection();
//                    //for testing
//                    //services.AddScoped<IUserSite, UserSite>((p) => { return new UserSite() { Host = new string[] { "" }, Name = Guid.NewGuid().ToString("N") }; });
//                    foreach (var item in proviteServices)
//                    {
//                        containerBuilder.RegisterType(item.ImplementationType).As(item.ServiceType);
//                    }
//                    //containerBuilder.Populate(proviteServices);
//                    //containerBuilder.RegisterModule<DefaultModule>();
//                    containerBuilder.RegisterAssemblyModules();

//                    var container = containerBuilder.Build();

//                    serviceProviderCache.Add(site.Name, container.Resolve<IServiceProvider>());
//                }
//                context.RequestServices = serviceProviderCache[site.Name];
//            }

//            return next(context);
//        }
//    }
//}