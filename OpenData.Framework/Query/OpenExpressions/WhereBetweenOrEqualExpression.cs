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
    public class WhereBetweenOrEqualExpression : WhereBetweenExpression
    {
        public WhereBetweenOrEqualExpression(IExpression expression, string fieldName, object start, object end)
            : base(expression, fieldName, start, end)
        { }

    }
}
