#region License
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
using System.Text;

namespace OpenData.ServiceProcess
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets or sets the starter.
        /// </summary>
        /// <value>
        /// The starter.
        /// </value>
        ApplicationStarter Starter { get; set; }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        bool Execute(string[] args);
    }
}
