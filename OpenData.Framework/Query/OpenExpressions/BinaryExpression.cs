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
    public class BinaryExpression : WhereFieldExpression
    {
        public BinaryExpression(IExpression expression, string fieldName, object value)
            : base(expression, fieldName)
        {
            this.Value = ExpressionValueHelper.Escape(value);

        }
        public object Value { get; private set; }
    }
}
