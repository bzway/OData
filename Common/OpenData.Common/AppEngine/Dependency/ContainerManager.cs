﻿#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Syntax;
using OpenData.AppEngine.Dependency.InRequestScope;
namespace OpenData.AppEngine.Dependency
{
    /// <summary>
    /// 加入Container Manager是为了减少其它程序在做注入的时候减少对Ninject的依赖
    /// 有了这个类以后，未来的扩展程序集在注册组件时就不用引用Ninject，对它形成依赖。
    /// </summary>
    public class ContainerManager : IDisposable
    {
        private IKernel _container;

        public ContainerManager()
        {
            _container = new StandardKernel();

            _container.Settings.Set("InjectAttribute", typeof(InjectAttribute));
        }

        /// <summary>
        /// Ninject
        /// </summary>
        public IKernel Container
        {
            get { return _container; }
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        public virtual void AddComponent<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            AddComponent<TService, TService>(key, lifeStyle);
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        public virtual void AddComponent(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            AddComponent(service, service, key, lifeStyle);
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        public virtual void AddComponent<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            AddComponent(typeof(TService), typeof(TImplementation), key, lifeStyle);
        }

        public virtual void AddComponent(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            _container.Bind(service).To(implementation).PerLifeStyle(lifeStyle).MapKey(key).ReplaceExisting(service);
        }

        public virtual void AddComponentInstance<TService>(object instance, string key = "")
        {
            AddComponentInstance(typeof(TService), instance, key);
        }
        public virtual void AddComponentInstance(object instance, string key = "")
        {
            AddComponentInstance(instance.GetType(), instance, key);
        }
        public virtual void AddComponentInstance(Type service, object instance, string key = "")
        {
            _container.Bind(service).ToConstant(instance).MapKey(key).ReplaceExisting(service);
        }

        public virtual T Resolve<T>(string key = "") where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Get<T>();
            }
            return _container.Get<T>(key);
        }

        public virtual object Resolve(Type type, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Get(type);
            }
            return _container.Get(type, key);
        }

        #region ResolveAll
        public virtual T[] ResolveAll<T>(string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.GetAll<T>().ToArray();
            }
            return _container.GetAll<T>(key).ToArray();
        }
        public virtual object[] ResolveAll(Type type, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.GetAll(type).ToArray();
            }
            return _container.GetAll(type, key).ToArray();
        }
        #endregion

        #region TryResolve
        public virtual T TryResolve<T>(string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.TryGet<T>();
            }
            return _container.TryGet<T>(key);
        }

        public virtual object TryResolve(Type type, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.TryGet(type);
            }
            return _container.TryGet(type, key);
        }

        #endregion

        public virtual T ResolveUnregistered<T>() where T : class
        {
            return ResolveUnregistered(typeof(T)) as T;
        }

        public virtual object ResolveUnregistered(Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {

                var parameters = constructor.GetParameters();
                var parameterInstances = new List<object>();
                foreach (var parameter in parameters)
                {
                    var service = Resolve(parameter.ParameterType);
                    if (service == null)
                        parameterInstances.Add(service);
                }
                return Activator.CreateInstance(type, parameterInstances.ToArray());


            }
            throw new Exception("No contructor was found that had all the dependencies satisfied.");
        }

        public void Dispose()
        {
            if (this._container != null && !this._container.IsDisposed)
            {
                this._container.Dispose();
            }

            this._container = null;
        }
    }
    public static class ContainerManagerExtensions
    {
        public static IBindingNamedWithOrOnSyntax<T> PerLifeStyle<T>(this IBindingWhenInNamedWithOrOnSyntax<T> binding, ComponentLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case ComponentLifeStyle.Singleton:
                    return binding.InSingletonScope();
                case ComponentLifeStyle.InThreadScope:
                    return binding.InThreadScope();
                case ComponentLifeStyle.InRequestScope:
                    return binding.InRequestScope();
                case ComponentLifeStyle.Transient:
                default:
                    return binding.InTransientScope();
            }
        }
        public static IBindingSyntax MapKey<T>(this IBindingNamedSyntax<T> binding, string key)
        {
            IBindingSyntax bindingSyntax = binding;
            if (!string.IsNullOrEmpty(key))
            {
                bindingSyntax = binding.Named(key);
            }

            return bindingSyntax;
        }
        public static void ReplaceExisting(this IBindingSyntax bindingInSyntax, Type type)
        {
            var kernel = bindingInSyntax.Kernel;
            var bindingsToRemove = kernel.GetBindings(type).Where(b => string.Equals(b.Metadata.Name, bindingInSyntax.BindingConfiguration.Metadata.Name, StringComparison.Ordinal));
            foreach (var bindingToRemove in bindingsToRemove)
            {
                kernel.RemoveBinding(bindingToRemove);
            }

            var binding = new Ninject.Planning.Bindings.Binding(type, bindingInSyntax.BindingConfiguration);
            kernel.AddBinding(binding);
        }
    }
}
