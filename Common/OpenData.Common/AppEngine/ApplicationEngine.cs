#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Autofac;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OpenData.Common.AppEngine
{

    /// <summary>
    /// Provides access to the singleton instance of the engine.
    /// </summary>
    public class ApplicationEngine
    {

        private IContainer container;
        private static ApplicationEngine _this;
        private static object lockObj = new object();
        private ApplicationEngine()
        {
            lock (lockObj)
            {
                if (_this == null)
                {
                    var containerBuilder = new ContainerBuilder();
                    this.container = containerBuilder.Build();
                }
            }
        }
        public static ApplicationEngine Current
        {
            get
            {
                if (_this == null)
                {
                    _this = new ApplicationEngine();
                }
                return _this;
            }
        }

        public IContainer this[string key]
        {
            get
            {
                if (container.IsRegisteredWithName<IContainer>(key))
                {
                    return container.ResolveNamed<IContainer>(key);
                }
                return null;
            }
            set
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterInstance<IContainer>(container).Named<IContainer>(key);
                containerBuilder.Update(container);
            }
        }
        public IContainer Default
        {
            get
            {
                if (!container.IsRegistered<IContainer>())
                {
                    return container;
                }
                return container.Resolve<IContainer>();
            }
        }
    }
}