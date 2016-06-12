using OpenData.Common;
using OpenData.Data.Query.OpenExpressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace OpenData.Data.Query
{
    public interface IWhere<T>
    {
        IWhereExpression Expression { get; }
        IWhere<T> Where<Key>(Expression<Func<T, Key>> keySelector, object value, CompareType type);
        IWhere<T> Where(string fieldName, object value, CompareType type);
        IWhere<T> Or(IWhere<T> query);
    }
}