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
    public class ItemAlreadyExistsException : FriendlyException
    {
        public ItemAlreadyExistsException()
            : base("The item already exists.")
        {

        }
    }
}
