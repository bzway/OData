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

namespace OpenData.Framework.Query.Expressions
{
    public class SelectExpression : Expression
    {
        public SelectExpression(IExpression expression, string[] fields)
            : base(expression)
        {
            this.Fields = fields;
        }
        public string[] Fields { get; private set; }        
    }
}
