#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using OpenData.AppEngine.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenData.Common
{
    #region IBaseDir
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseDir
    {
        /// <summary>
        /// Gets the CMS base dir.
        /// </summary>
        /// <value>
        /// The CMS base dir.
        /// </value>
        string AppBaseDirectory { get; }
        /// <summary>
        /// Gets the name of the CMS_ data path.
        /// </summary>
        /// <value>
        /// The name of the CMS_ data path.
        /// </value>
        string AppDataPathName { get; }
        /// <summary>
        /// Gets the CMS_data base path.
        /// </summary>
        /// <value>
        /// The CMS_data base path.
        /// </value>
        string AppDataPhysicalPath { get; }

        /// <summary>
        /// Gets the CMS_data virutal path.
        /// </summary>
        /// <value>
        /// The CMS_ data virutal path.
        /// </value>
        string AppDataVirutalPath { get; }

        /// <summary>
        /// Gets the name of the setting file.
        /// </summary>
        /// <value>
        /// The name of the setting file.
        /// </value>
        string SettingFileName { get; }
    }
    #endregion

    #region BaseDir
    /// <summary>
    /// 
    /// </summary>
    [Dependency(typeof(IBaseDir))]
    public class BaseDir : IBaseDir
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDir" /> class.
        /// </summary>
        public BaseDir()
        {
            this.AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.AppDataPathName = "App_Data";
            this.AppDataPhysicalPath = Path.Combine(AppBaseDirectory, this.AppDataPathName);
            this.AppDataVirutalPath = UrlUtility.Combine("~/", this.AppDataPathName);
        }

        /// <summary>
        /// Gets the CMS base dir.
        /// </summary>
        /// <value>
        /// The CMS base dir.
        /// </value>
        public string AppBaseDirectory
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the name of the CMS_ data path.
        /// </summary>
        /// <value>
        /// The name of the CMS_ data path.
        /// </value>
        public string AppDataPathName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the CMS_ data base path.
        /// </summary>
        /// <value>
        /// The CMS_ data base path.
        /// </value>
        public string AppDataPhysicalPath
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the name of the setting file.
        /// </summary>
        /// <value>
        /// The name of the setting file.
        /// </value>
        public string SettingFileName
        {
            get { return "setting.config"; }
        }


        /// <summary>
        /// Gets or sets the CMS_ data virutal path.
        /// </summary>
        public string AppDataVirutalPath
        {
            get;
            set;
        }
    }
    #endregion
}
