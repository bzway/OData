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
using System.Threading.Tasks;

namespace OpenData.Web.Common.Persistence.Non_Relational
{
    public interface IIdentifiable
    {
        string UUID { get; set; }
    }
}
