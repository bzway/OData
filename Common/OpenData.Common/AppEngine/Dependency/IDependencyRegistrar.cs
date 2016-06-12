#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ninject;

namespace OpenData.AppEngine.Dependency
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerManager containerManager, ITypeFinder typeFinder);

        int Order { get; }
    }
}
