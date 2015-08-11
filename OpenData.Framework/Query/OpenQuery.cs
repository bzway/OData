using OpenData.Framework.Query.OpenExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Framework
{

    public class OpenQuery<T> : IOpenQuery<T> where T : OpenEntity
    {
        public OpenQuery(IOpenRepository<T> repository) : this(repository, null) { }

        public OpenQuery(IOpenRepository<T> repository, IOpenExpression OpenExpression)
        {
            this.OpenExpression = OpenExpression;
            this.Repository = repository;
        }
        public string Name { get; private set; }
        public IOpenExpression OpenExpression
        {
            get;
            private set;
        }

        public IOpenQuery<T> Select(params string[] fields)
        {
            var OpenExpression = new SelectOpenExpression(this.OpenExpression, fields);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> Skip(int count)
        {
            var OpenExpression = new SkipOpenExpression(this.OpenExpression, count);
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> Take(int count)
        {
            var OpenExpression = new TakeOpenExpression(this.OpenExpression, count);
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> OrderBy(string fieldName)
        {
            var OpenExpression = new OrderOpenExpression(this.OpenExpression, fieldName, false);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> OrderByDescending(string fieldName)
        {
            var OpenExpression = new OrderOpenExpression(this.OpenExpression, fieldName, true);
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> OrderBy(OrderOpenExpression OpenExpression)
        {
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> Or(IWhereOpenExpression OpenExpression)
        {
            IOpenExpression exp = null;
            if (this.OpenExpression is IWhereOpenExpression)
            {
                exp = new OrElseOpenExpression((IWhereOpenExpression)this.OpenExpression, OpenExpression);
            }
            else
            {
                exp = new OrElseOpenExpression(new FalseOpenExpression(), OpenExpression);
            }
            return this.Create(exp);
        }
        public IOpenQuery<T> Where(IWhereOpenExpression OpenExpression)
        {
            IOpenExpression exp = null;
            if (this.OpenExpression is IWhereOpenExpression)
            {
                exp = new AndAlsoOpenExpression((IWhereOpenExpression)OpenExpression, OpenExpression);
            }
            else
            {
                exp = new AndAlsoOpenExpression(new TrueOpenExpression(), OpenExpression);
            }

            return this.Create(exp);
        }
        public IOpenQuery<T> Where(string whereClause)
        {
            var OpenExpression = new WhereClauseOpenExpression(this.OpenExpression, whereClause);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereBetween(string fieldName, object start, object end)
        {
            var OpenExpression = new WhereBetweenOpenExpression(this.OpenExpression, fieldName, start, end);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereBetweenOrEqual(string fieldName, object start, object end)
        {
            var OpenExpression = new WhereBetweenOrEqualOpenExpression(this.OpenExpression, fieldName, start, end);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereContains(string fieldName, object value)
        {
            var OpenExpression = new WhereContainsOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereEndsWith(string fieldName, object value)
        {
            var OpenExpression = new WhereEndsWithOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereEquals(string fieldName, object value)
        {
            var OpenExpression = new WhereEqualsOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> WhereNotEquals(string fieldName, object value)
        {
            var OpenExpression = new WhereNotEqualsOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereGreaterThan(string fieldName, object value)
        {
            var OpenExpression = new WhereGreaterThanOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereGreaterThanOrEqual(string fieldName, object value)
        {
            var OpenExpression = new WhereGreaterThanOrEqualOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereLessThan(string fieldName, object value)
        {
            var OpenExpression = new WhereLessThanOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereLessThanOrEqual(string fieldName, object value)
        {
            var OpenExpression = new WhereLessThanOrEqualOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }

        public IOpenQuery<T> WhereStartsWith(string fieldName, object value)
        {
            var OpenExpression = new WhereStartsWithOpenExpression(this.OpenExpression, fieldName, value);
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> WhereIn(string fieldName, params object[] values)
        {
            //IWhereOpenExpression exp = new FalseOpenExpression();
            //foreach (var value in values)
            //{
            //    exp = new OrElseOpenExpression(exp, new WhereEqualsOpenExpression(null, fieldName, value));
            //}
            //return this.Where(exp);
            var OpenExpression = new WhereInOpenExpression(this.OpenExpression, fieldName, values);
            return this.Create(OpenExpression);
        }
        public IOpenQuery<T> WhereNotIn(string fieldName, params object[] values)
        {
            var OpenExpression = new WhereNotInOpenExpression(this.OpenExpression, fieldName, values);
            return this.Create(OpenExpression);
        }
        public int Count()
        {
            var contentQuery = this.Create(new CallOpenExpression(this.OpenExpression, CallType.Count));
            return (int)this.Repository.Execute(contentQuery);
        }

        public T First()
        {
            var contentQuery = this.Create(new CallOpenExpression(this.OpenExpression, CallType.First));
            return (T)this.Repository.Execute(contentQuery);
        }

        public T FirstOrDefault()
        {
            var contentQuery = this.Create(new CallOpenExpression(this.OpenExpression, CallType.FirstOrDefault));
            return (T)this.Repository.Execute(contentQuery);
        }

        public T Last()
        {
            var contentQuery = this.Create(new CallOpenExpression(this.OpenExpression, CallType.Last));
            return (T)this.Repository.Execute(contentQuery);
        }

        public T LastOrDefault()
        {
            var contentQuery = this.Create(new CallOpenExpression(this.OpenExpression, CallType.LastOrDefault));
            return (T)this.Repository.Execute(contentQuery);
        }


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.Repository.Execute(this)).GetEnumerator();
        }

        #endregion



        #region IOpenQuery<T> Members


        public virtual IOpenQuery<T> Create(IOpenExpression OpenExpression)
        {
            return new OpenQuery<T>(this.Repository, OpenExpression);
        }

        #endregion




        public IOpenRepository<T> Repository
        {
            get;
            set;
        }


        public IEnumerable<T> ToList()
        {
            return (IEnumerable<T>)this.Repository.Execute(this);
        }


    }
    public static class extine
    {
        //public static IOpenQuery<T> Where(this IEnumerable<T> source, Func<T, bool> predicate)


        public static IOpenQuery<TSource> WhereTo<TSource>(this IOpenQuery<TSource> source, Func<TSource, bool> predicate) where TSource : OpenEntity
        {
            var a = predicate;
            return null;
        }
    }
}