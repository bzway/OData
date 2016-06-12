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

namespace OpenData.Common
{

    [Serializable]
    public class FriendlyException : Exception
    {
        public FriendlyException(string msg) : base(msg) { }
        public FriendlyException(string msg, Exception inner) : base(msg, inner) { }
    }
}
