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

namespace OpenData.Framework.Query.OpenExpressions
{
    public class SelectOpenExpression : OpenExpression
    {
        public SelectOpenExpression(IOpenExpression OpenExpression, string[] fields)
            : base(OpenExpression)
        {
            this.Fields = fields;
        }
        public string[] Fields { get; private set; }        
    }
}
