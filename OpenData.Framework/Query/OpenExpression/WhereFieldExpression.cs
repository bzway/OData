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
    public abstract class WhereFieldOpenExpression : OpenExpression, IWhereOpenExpression
    {
        public WhereFieldOpenExpression(IOpenExpression OpenExpression, string fieldName)
            : base(OpenExpression)
        {
            this.FieldName = fieldName;
        }
        public string FieldName { get; private set; }

    }
}
