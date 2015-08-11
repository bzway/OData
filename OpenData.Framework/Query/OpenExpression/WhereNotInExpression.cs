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
    public class WhereNotInOpenExpression : WhereFieldOpenExpression
    {
        public WhereNotInOpenExpression(IOpenExpression OpenExpression, string fieldName, object[] values)
            : base(OpenExpression, fieldName)
        {
            this.Values = values;
        }
        public object[] Values { get; set; }
    }
}
