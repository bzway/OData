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
    public enum CallType
    {
        Unspecified,
        Count,
        First,
        Last,
        LastOrDefault,
        FirstOrDefault        
    }
    public class CallOpenExpression : OpenExpression
    {
        public CallOpenExpression(IOpenExpression OpenExpression, CallType type)
            : base(OpenExpression)
        {
            this.CallType = type;
        }
        public CallType CallType { get; private set; }
    }
}
