using OpenData.Framework.Query;
using OpenData.Framework.Query.Expressions;
using System.Collections.Generic;

namespace OpenData.Framework
{

    public abstract class ExpressionVisitor
    {

        #region ctor


        public ExpressionVisitor()
        {
            this.OrderFields = new List<OrderField>();
            this.Filters = new List<Filter>();
            this.SelectFields = new List<string>();
        }
        public int Top { get; set; }


        public List<OrderField> OrderFields { get; set; }
        public List<Filter> Filters { get; set; }

        public List<string> SelectFields { get; set; }

        #endregion

        public virtual void Visite(IExpression expression)
        {
            if (expression == null)
            {
                return;
            }
            if (expression is Expression && ((Expression)expression).InnerExpression != null)
            {
                this.Visite(((Expression)expression).InnerExpression);
            }

            VisiteExpression(expression);
        }

        protected virtual void VisiteExpression(IExpression expression)
        {
            if (expression is CallExpression)
            {
                VisitCall((CallExpression)expression);
            }
            else if (expression is OrderExpression)
            {
                VisitOrder((OrderExpression)expression);
            }
            else if (expression is SelectExpression)
            {
                VisitSelect((SelectExpression)expression);
            }
            else if (expression is SkipExpression)
            {
                VisitSkip((SkipExpression)expression);
            }
            else if (expression is TakeExpression)
            {
                VisitTake((TakeExpression)expression);
            }
            //else if (expression is WhereCategoryExpression)
            //{
            //    VisitWhereCategory((WhereCategoryExpression)expression);
            //}
            else if (expression is IWhereExpression)
            {
                VisitWhere((IWhereExpression)expression);
            }
        }

        //public virtual void ReverseOrder()
        //{
        //    this.orderFields.Reverse();
        //}

        protected abstract void VisitSkip(SkipExpression expression);
        protected abstract void VisitTake(TakeExpression expression);

        protected virtual void VisitSelect(SelectExpression expression)
        {
            this.SelectFields.AddRange(expression.Fields);
        }

        protected virtual void VisitOrder(OrderExpression expression)
        {
            this.OrderFields.Add(new OrderField() { FieldName = expression.FieldName, Descending = expression.Descending });
        }
        protected virtual void VisitWhereIn(WhereInExpression expression)
        {
            IWhereExpression exp = new FalseExpression();
            foreach (var value in expression.Values)
            {
                exp = new OrElseExpression(exp, new WhereEqualsExpression(null, expression.FieldName, value));
            }
            this.VisitWhere(exp);
        }
        protected virtual void VisitWhereNotIn(WhereNotInExpression expression)
        {
            IWhereExpression exp = new TrueExpression();
            foreach (var value in expression.Values)
            {
                exp = new AndAlsoExpression(exp, new WhereNotEqualsExpression(null, expression.FieldName, value));
            }
            this.VisitWhere(exp);
        }

        protected virtual void VisitCall(CallExpression expression)
        {
            if (expression.CallType == CallType.First || expression.CallType == CallType.FirstOrDefault || expression.CallType == CallType.Last || expression.CallType == CallType.LastOrDefault)
            {
                this.Top = 1;
            }
        }

        //protected abstract void VisitWhereCategory(WhereCategoryExpression expression);

        protected abstract void VisitWhereBetweenOrEqual(WhereBetweenOrEqualExpression expression);
        protected abstract void VisitWhereBetween(WhereBetweenExpression expression);
        protected abstract void VisitWhereContains(WhereContainsExpression expression);
        protected abstract void VisitWhereEndsWith(WhereEndsWithExpression expression);
        protected virtual void VisitWhereEquals(WhereEqualsExpression expression)
        {
            this.Filters.Add(new Filter()
            {
                ConditionType = System.Linq.Expressions.ExpressionType.Equal,
                Name = expression.FieldName,
                Value = expression.Value.ToString(),
            });
        }
        protected virtual void VisitWhereClause(WhereClauseExpression expression)
        {
            this.Filters.Add(new Filter { Name = "tes", Value = expression.WhereClause, ConditionType = System.Linq.Expressions.ExpressionType.Equal });
        }
        protected abstract void VisitWhereGreaterThan(WhereGreaterThanExpression expression);
        protected abstract void VisitWhereGreaterThanOrEqual(WhereGreaterThanOrEqualExpression expression);

        protected abstract void VisitWhereLessThan(WhereLessThanExpression expression);
        protected abstract void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression);

        protected abstract void VisitWhereStartsWith(WhereStartsWithExpression expression);
        protected abstract void VisitWhereNotEquals(WhereNotEqualsExpression expression);

        protected abstract void VisitAndAlso(AndAlsoExpression expression);
        protected abstract void VisitOrElse(OrElseExpression expression);
        protected abstract void VisitFalse(FalseExpression expression);
        protected abstract void VisitTrue(TrueExpression expression);

        protected virtual void VisitWhere(IWhereExpression expression)
        {
            if (expression is TrueExpression)
            {
                VisitTrue((TrueExpression)expression);
            }
            else if (expression is FalseExpression)
            {
                VisitFalse((FalseExpression)expression);
            }
            if (expression is WhereBetweenOrEqualExpression)
            {
                var whereBetweenExpression = (WhereBetweenOrEqualExpression)expression;
                VisitWhereBetweenOrEqual(whereBetweenExpression);
            }
            else if (expression is WhereBetweenExpression)
            {
                var whereBetweenExpression = (WhereBetweenExpression)expression;
                VisitWhereBetween(whereBetweenExpression);

            }
            else if (expression is WhereContainsExpression)
            {
                var whereContainsExpression = (WhereContainsExpression)expression;
                VisitWhereContains(whereContainsExpression);
            }
            else if (expression is WhereEndsWithExpression)
            {
                var whereEndsWithExpression = (WhereEndsWithExpression)expression;
                VisitWhereEndsWith(whereEndsWithExpression);
            }
            else if (expression is WhereEqualsExpression)
            {
                var whereEqualsExpression = (WhereEqualsExpression)expression;
                VisitWhereEquals(whereEqualsExpression);
            }
            else if (expression is WhereClauseExpression)
            {
                var whereExpression = (WhereClauseExpression)expression;
                VisitWhereClause(whereExpression);
            }
            else if (expression is WhereGreaterThanExpression)
            {
                var whereGreaterThan = (WhereGreaterThanExpression)expression;
                VisitWhereGreaterThan(whereGreaterThan);
            }
            else if (expression is WhereGreaterThanOrEqualExpression)
            {
                var whereGreaterThenOrEquals = (WhereGreaterThanOrEqualExpression)expression;
                VisitWhereGreaterThanOrEqual(whereGreaterThenOrEquals);
            }
            else if (expression is WhereLessThanExpression)
            {
                var whereLessThan = (WhereLessThanExpression)expression;
                VisitWhereLessThan(whereLessThan);
            }
            else if (expression is WhereLessThanOrEqualExpression)
            {
                var whereLessThanOrEqual = (WhereLessThanOrEqualExpression)expression;
                VisitWhereLessThanOrEqual(whereLessThanOrEqual);
            }
            else if (expression is WhereNotEqualsExpression)
            {
                VisitWhereNotEquals((WhereNotEqualsExpression)expression);
            }
            else if (expression is WhereStartsWithExpression)
            {
                var whereStartWith = (WhereStartsWithExpression)expression;
                VisitWhereStartsWith(whereStartWith);
            }
            else if (expression is WhereInExpression)
            {
                var whereInExpression = (WhereInExpression)expression;
                VisitWhereIn(whereInExpression);
            }
            else if (expression is WhereNotInExpression)
            {
                var whereNotInExpression = (WhereNotInExpression)expression;
                VisitWhereNotIn(whereNotInExpression);
            }
            else if (expression is OrElseExpression)
            {
                VisitOrElse((OrElseExpression)expression);
            }
            else if (expression is AndAlsoExpression)
            {
                VisitAndAlso((AndAlsoExpression)expression);
            }
        }
    }

}
