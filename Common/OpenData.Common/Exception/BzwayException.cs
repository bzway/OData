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

namespace OpenData
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BzwayException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BzwayException" /> class.
        /// </summary>
        public BzwayException()
            : base()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BzwayException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public BzwayException(string msg)
            : base(msg)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BzwayException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="exception">The exception.</param>
        public BzwayException(string msg, Exception exception)
            : base(msg, exception)
        { }
    }
}
