﻿using OpenData.Message;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OpenData.Framework.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            SohuSMTPService smtp = new SohuSMTPService();
            smtp.SendMail("adm.zhu@bzway.com", "zhumingwu@126.com;zhumingwu@hotmail.com", "测试邮件", "这是一个测试邮件<a href='http://bzway.com'>click here</a>");
            //publishMessage();
            //receiveMessage();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //var controllerFactory = new BzwayControllerFactory();
            //ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            //ModelMetadataProviders.Current = new BzwayDataAnnotationsModelMetadataProvider() { };
            //ModelValidatorProviders.Providers.Add(new MyModelValidatorProvider());
            //ModelBinderProviders.BinderProviders.Add(new EntityModelBinder());


            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new BzwayViewEngine());
            Log4NetConfig.Register(this);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            log.Error("Exception:", ex);
            //Server.ClearError();
            //this.Response.Redirect("~/error.html");
        }

        public static void publishMessage()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "172.31.212.36";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            factory.UserName = "justin";
            factory.Password = "abc123$";
            //factory.VirtualHost = "gyg001";
            factory.RequestedHeartbeat = 30;
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var aaa = channel.QueueDeclare("hello", true, false, false, null);

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    for (int i = 0; i < 10; i++)
                    {

                        string message = "Hello World!" + i.ToString();
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish("", "hello", null, body);

                    }
                }
            }
        }



        public static void receiveMessage()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "172.31.212.36";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            factory.UserName = "justin";
            factory.Password = "abc123$";
            //factory.VirtualHost = "gyg001";
            factory.RequestedHeartbeat = 30;
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {

                    //普通使用方式BasicGet  
                    //noAck = true，不需要回复，接收到消息后，queue上的消息就会清除  
                    //noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，  
                    //直到调用channel.basicAck(deliveryTag, false);   
                    //queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息  

                    BasicGetResult res = channel.BasicGet("hello", false/*noAck*/);
                    if (res != null)
                    {
                        var message = UTF8Encoding.UTF8.GetString(res.Body);
                        channel.BasicAck(res.DeliveryTag, false);
                    }
                }
            }
        }
    }
}