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
    public class OrElseOpenExpression : IWhereOpenExpression
    {
        public OrElseOpenExpression(IWhereOpenExpression left, IWhereOpenExpression right)
        {
            this.Left = left;
            this.Right = right;
        }
        public IWhereOpenExpression Left { get; private set; }
        public IWhereOpenExpression Right { get; private set; }
    }
}
