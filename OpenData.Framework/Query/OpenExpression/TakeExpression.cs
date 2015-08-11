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
    public class TakeOpenExpression : OpenExpression
    {
        public TakeOpenExpression(IOpenExpression OpenExpression, int count)
            : base(OpenExpression)
        {
            this.Count = count;
        }
        public int Count { get; set; }
    }
}
