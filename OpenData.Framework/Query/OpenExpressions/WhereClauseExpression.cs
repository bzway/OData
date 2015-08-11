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
    public class WhereClauseExpression : Expression, IWhereExpression
    {
        public WhereClauseExpression(IExpression expression, string whereClause)
            : base(expression)
        {
            this.WhereClause = whereClause;
        }
        public string WhereClause { get; private set; }
    }
}
