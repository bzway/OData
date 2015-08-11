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

namespace OpenData.Framework.Query.OpenExpressions
{
    public abstract class OpenExpression : IOpenExpression
    {
        public OpenExpression(IOpenExpression OpenExpression)
        {
            this.InnerOpenExpression = OpenExpression;
        }
        public IOpenExpression InnerOpenExpression { get; private set; }
    }
}
