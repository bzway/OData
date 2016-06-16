#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Autofac;

namespace OpenData.Common.AppEngine
{
    public class ITypeActivtorStartupTask : IStartupTask
    {
        public void Execute()
        {
            TypeActivator.CreateInstanceMethod = (type) =>
            {
                return ApplicationEngine.Current.Default.Resolve(type);
            };
        }

        public int Order
        {
            get { return 0; }
        }
    }
}