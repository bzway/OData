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
    public class OrderOpenExpression : WhereFieldOpenExpression
    {
        public OrderOpenExpression(IOpenExpression OpenExpression, string fieldName, bool descending) :
            base(OpenExpression,fieldName)
        {
            this.Descending = descending;
        }
        
        public bool Descending { get; set; }
    }
}
