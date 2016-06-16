#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
namespace OpenData.Common.AppEngine
{
    public enum ComponentLifeStyle
    {
        Singleton = 0,
        Transient = 1,
        InThreadScope = 2,
        InRequestScope = 3
    }
}
