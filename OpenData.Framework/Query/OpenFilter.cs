using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OpenData.Framework.Query
{
    public class OpenFilter
    {
        public string Name { get; set; }
        public ExpressionType ConditionType { get; set; }
        public string Value { get; set; }

    }
}