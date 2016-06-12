#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace OpenData.AppEngine
{
    public class ITypeActivtorStartupTask : IStartupTask
    {
        public void Execute()
        {
            TypeActivator.CreateInstanceMethod = (type) =>
            {
                return ApplicationEngine.Current.Resolve(type);
            };
        }

        public int Order
        {
            get { return 0; }
        }
    }
}