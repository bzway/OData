﻿#region License
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
using System.Web;

namespace OpenData.Website.Form.Html.Controls
{
    public class Password : Input
    {
        public override string Type
        {
            get { return "Password"; }
        }

        public override string Name
        {
            get { return "Password"; }
        }
    }
}
