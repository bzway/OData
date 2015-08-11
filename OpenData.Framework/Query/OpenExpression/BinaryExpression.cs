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
    public class BinaryOpenExpression : WhereFieldOpenExpression
    {
        public BinaryOpenExpression(IOpenExpression OpenExpression, string fieldName, object value)
            : base(OpenExpression, fieldName)
        {
            this.Value = OpenExpressionValueHelper.Escape(value);

        }
        public object Value { get; private set; }
    }
}
