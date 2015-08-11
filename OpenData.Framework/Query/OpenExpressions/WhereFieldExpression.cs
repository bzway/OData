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
    public abstract class WhereFieldExpression : Expression, IWhereExpression
    {
        public WhereFieldExpression(IExpression expression, string fieldName)
            : base(expression)
        {
            this.FieldName = fieldName;
        }
        public string FieldName { get; private set; }

    }
}
