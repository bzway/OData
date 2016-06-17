#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using Autofac;
using System.Linq;
using System.Collections.Generic;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using System.Web.Mvc;

namespace OpenData.Common.AppEngine
{

    public class ContainerManager : Autofac.Module, IContainerManager
    {
        ContainerBuilder builder;
        protected override void Load(ContainerBuilder builder)
        {
            this.builder = builder;
            WebAppTypeFinder typeFinder = new WebAppTypeFinder();
            var types = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            List<IDependencyRegistrar> list = new List<IDependencyRegistrar>();
            foreach (var item in types)
            {
                list.Add((IDependencyRegistrar)TypeActivator.CreateInstance(item));
            }

            foreach (var item in list.OrderBy(m => m.Order))
            {
                item.Register(this, typeFinder);
            }
            types = typeFinder.FindClassesOfType<IController>();
            foreach (var item in types)
            {
                builder.RegisterType(item);
            }
            base.Load(builder);
        }

        public void RegisterInstance(Type service, object instance, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterInstance(instance).As(service);
            }
            else
            {
                builder.RegisterInstance(instance).As(service).Named(key, service);
            }

        }

        public void RegisterInstance<TService>(object instance, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterInstance(instance).As<TService>();
            }
            else
            {
                builder.RegisterInstance(instance).As<TService>().Named<TService>(key);
            }
        }

        public void RegisterType(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterType(service);
            }
            else
            {
                builder.RegisterType(service).Named(key, service);
            }
        }

        public void RegisterType(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterType(implementation).As(service);
            }
            else
            {
                builder.RegisterType(implementation).As(service).Named(key, service);
            }
        }

        public void RegisterType<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterType<TService>();
            }
            else
            {
                builder.RegisterType<TService>().Named<TService>(key);
            }
        }

        public void RegisterType<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterType<TImplementation>().As<TService>();
            }
            else
            {
                builder.RegisterType<TImplementation>().As<TService>().Named<TService>(key);
            }
        }
    }
}