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
    public class OrderExpression : WhereFieldExpression
    {
        public OrderExpression(IExpression expression, string fieldName, bool descending) :
            base(expression,fieldName)
        {
            this.Descending = descending;
        }
        
        public bool Descending { get; set; }
    }
}
