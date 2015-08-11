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
using System.Text;

namespace OpenData.Framework.Query.Expressions
{
    public abstract class Expression : IExpression
    {
        public Expression(IExpression expression)
        {
            this.InnerExpression = expression;
        }
        public IExpression InnerExpression { get; private set; }
    }
}
