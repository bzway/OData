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
    public interface IStartupTask 
    {
        void Execute();

        int Order { get; }
    }
}
