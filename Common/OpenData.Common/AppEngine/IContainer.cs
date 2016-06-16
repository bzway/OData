using System;
using Autofac;

namespace OpenData.Common.AppEngine
{
    public interface IContainerManager : IContainer
    {
        void RegisterInstance(Type service, object instance, string key = "");
        void RegisterInstance<TService>(object instance, string key = "");
        void RegisterType(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);
        void RegisterType(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);
        void RegisterType<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);
        void RegisterType<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);
    }
}