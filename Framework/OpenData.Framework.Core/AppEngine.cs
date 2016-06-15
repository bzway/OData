
using Autofac;

namespace OpenData.Framework.Common
{
    public class BzwayAppEngine
    {
        private IContainer container;
        private static BzwayAppEngine _this;
        public static BzwayAppEngine Container
        {
            get
            {
                if (_this == null)
                {
                    _this = new BzwayAppEngine();
                }
                return _this;
            }
        }
        private BzwayAppEngine()
        {
            lock (_this)
            {
                if (_this == null)
                {
                    var containerBuilder = new ContainerBuilder();
                    this.container = containerBuilder.Build();
                }
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
                return container.Resolve<IContainer>();
            }
        }
    }
}