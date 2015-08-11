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
    public class WhereBetweenOpenExpression : WhereFieldOpenExpression
    {
        public WhereBetweenOpenExpression(IOpenExpression OpenExpression, string fieldName, object start, object end)
            : base(OpenExpression, fieldName)
        {
            this.Start = OpenExpressionValueHelper.Escape(start);
            this.End = OpenExpressionValueHelper.Escape(end);
        }
        public object Start { get; set; }
        public object End { get; private set; }
    }
}
