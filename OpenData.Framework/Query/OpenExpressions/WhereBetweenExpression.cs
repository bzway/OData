﻿#region License
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
    public class WhereBetweenExpression : WhereFieldExpression
    {
        public WhereBetweenExpression(IExpression expression, string fieldName, object start, object end)
            : base(expression, fieldName)
        {
            this.Start = ExpressionValueHelper.Escape(start);
            this.End = ExpressionValueHelper.Escape(end);
        }
        public object Start { get; set; }
        public object End { get; private set; }
    }
}
