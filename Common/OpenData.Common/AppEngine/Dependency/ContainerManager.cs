using System;
using System.Collections.Generic;
using Autofac;

namespace OpenData.AppEngine.Dependency
{
    /// <summary>
    /// 加入Container Manager是为了减少其它程序在做注入的时候减少对Ninject的依赖
    /// 有了这个类以后，未来的扩展程序集在注册组件时就不用引用Ninject，对它形成依赖。
    /// </summary>
    public class ContainerManager : IDisposable
    {
        private IContainer _container;

        public ContainerManager()
        {
            _container = new ContainerBuilder().Build();


        }

        /// <summary>
        /// Ninject
        /// </summary>
        public IContainer Container
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
            ContainerBuilder builder = new ContainerBuilder();
            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterInstance(implementation).As(service).OwnedByLifetimeScope();
            }
            else
            {
                builder.RegisterInstance(implementation).As(service).Named(key, service).OwnedByLifetimeScope();

            }
            builder.Update(_container);
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
            ContainerBuilder builder = new ContainerBuilder();

            if (string.IsNullOrEmpty(key))
            {
                builder.RegisterInstance(instance).As(service).OwnedByLifetimeScope();
            }
            else
            {
                builder.RegisterInstance(instance).As(service).Named(key, service).OwnedByLifetimeScope();
            }

            builder.Update(_container);
        }

        public virtual T Resolve<T>(string key = "") where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Resolve<T>();
            }
            return _container.ResolveNamed<T>(key);
        }

        public virtual object Resolve(Type type, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Resolve(type);
            }
            return _container.ResolveNamed(key, type);
        }

        #region ResolveAll
        public virtual T[] ResolveAll<T>(string key = "")
        {
            return null;
        }
        public virtual object[] ResolveAll(Type type, string key = "")
        {
            return null;
        }
        #endregion

        #region TryResolve
        public virtual T TryResolve<T>(string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                T o;
                _container.TryResolve<T>(out o);
                return o;
            }
            object ob;
            _container.TryResolveNamed(key, typeof(T), out ob);
            return (T)ob;
        }

        public virtual object TryResolve(Type type, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                object o;
                _container.TryResolve(type, out o);
                return o;
            }
            object ob;
            _container.TryResolveNamed(key, type, out ob);
            return ob;
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
            if (this._container != null)
            {
                this._container.Dispose();
            }

            this._container = null;
        }
    }
}