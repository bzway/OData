#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;
[assembly: PreApplicationStartMethod(typeof(OpenData.ApplicationEngineConfiguration), "Configuration")]
namespace OpenData
{
    public class ApplicationEngineConfiguration
    {
        public static void Configuration()
        {
            ApplicationEngine.Current.Initialize();
        }
    }
}