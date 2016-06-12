#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace OpenData.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class FileConfigurationSection : ConfigurationSection
    {
        #region Methods
        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sectionName">Name of the section.</param>
        public virtual void GetSection(string fileName, string sectionName)
        {
            XDocument dom = XDocument.Load(fileName);
            base.DeserializeSection(dom.Elements().Where(e => e.Name == sectionName).First().CreateReader());
        }
        #endregion
    }
}
