using OpenData.Framework.Query.OpenExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace OpenData.Framework
{
    public interface IOpenQuery<T> where T : OpenEntity
    {
        IOpenExpression OpenExpression { get; }
        //IOpenRepository<T> Repository { get; set; }
        IOpenQuery<T> Select(params string[] fields);

        IOpenQuery<T> Skip(int count);

        IOpenQuery<T> OrderBy(string field);
        IOpenQuery<T> OrderByDescending(string field);
        IOpenQuery<T> OrderBy(OrderOpenExpression OpenExpression);

        IOpenQuery<T> Or(IWhereOpenExpression OpenExpression);
        IOpenQuery<T> Where(IWhereOpenExpression OpenExpression);
        IOpenQuery<T> Where(string whereClause);
        IOpenQuery<T> WhereBetween(string fieldName, object start, object end);
        IOpenQuery<T> WhereBetweenOrEqual(string fieldName, object start, object end);
        IOpenQuery<T> WhereContains(string fieldName, object value);
        IOpenQuery<T> WhereEndsWith(string fieldName, object value);
        IOpenQuery<T> WhereEquals(string fieldName, object value);
        IOpenQuery<T> WhereNotEquals(string fieldName, object value);
        IOpenQuery<T> WhereGreaterThan(string fieldName, object value);
        IOpenQuery<T> WhereGreaterThanOrEqual(string fieldName, object value);
        IOpenQuery<T> WhereLessThan(string fieldName, object value);
        IOpenQuery<T> WhereLessThanOrEqual(string fieldName, object value);
        IOpenQuery<T> WhereStartsWith(string fieldName, object value);
        IOpenQuery<T> WhereIn(string fieldName, params object[] values);
        IOpenQuery<T> WhereNotIn(string fieldName, params object[] values);
        IOpenQuery<T> Take(int count);

        int Count();
        T First();
        T FirstOrDefault();
        T Last();
        T LastOrDefault();

        IEnumerable<T> ToList();

        IOpenQuery<T> Create(IOpenExpression OpenExpression);
    }
}