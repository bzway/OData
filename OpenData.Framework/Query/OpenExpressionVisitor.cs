using OpenData.Framework.Query;
using OpenData.Framework.Query.OpenExpressions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OpenData.Framework
{

    public abstract class OpenExpressionVisitor
    {

        #region ctor


        public OpenExpressionVisitor()
        {
            this.OrderFields = new List<OrderField>();
            this.Filters = new List<OpenFilter>();
            this.SelectFields = new List<string>();
        }
        public int Top { get; set; }


        public List<OrderField> OrderFields { get; set; }
        public List<OpenFilter> Filters { get; set; }

        public List<string> SelectFields { get; set; }

        #endregion

        public virtual void Visite(IOpenExpression OpenExpression)
        {
            if (OpenExpression == null)
            {
                return;
            }
            if (OpenExpression is OpenExpression && ((OpenExpression)OpenExpression).InnerOpenExpression != null)
            {
                this.Visite(((OpenExpression)OpenExpression).InnerOpenExpression);
            }

            VisiteOpenExpression(OpenExpression);
        }

        protected virtual void VisiteOpenExpression(IOpenExpression OpenExpression)
        {
            if (OpenExpression is CallOpenExpression)
            {
                VisitCall((CallOpenExpression)OpenExpression);
            }
            else if (OpenExpression is OrderOpenExpression)
            {
                VisitOrder((OrderOpenExpression)OpenExpression);
            }
            else if (OpenExpression is SelectOpenExpression)
            {
                VisitSelect((SelectOpenExpression)OpenExpression);
            }
            else if (OpenExpression is SkipOpenExpression)
            {
                VisitSkip((SkipOpenExpression)OpenExpression);
            }
            else if (OpenExpression is TakeOpenExpression)
            {
                VisitTake((TakeOpenExpression)OpenExpression);
            }
            //else if (OpenExpression is WhereCategoryOpenExpression)
            //{
            //    VisitWhereCategory((WhereCategoryOpenExpression)OpenExpression);
            //}
            else if (OpenExpression is IWhereOpenExpression)
            {
                VisitWhere((IWhereOpenExpression)OpenExpression);
            }
        }

        //public virtual void ReverseOrder()
        //{
        //    this.orderFields.Reverse();
        //}

        protected abstract void VisitSkip(SkipOpenExpression OpenExpression);
        protected abstract void VisitTake(TakeOpenExpression OpenExpression);

        protected virtual void VisitSelect(SelectOpenExpression OpenExpression)
        {
            this.SelectFields.AddRange(OpenExpression.Fields);
        }

        protected virtual void VisitOrder(OrderOpenExpression OpenExpression)
        {
            this.OrderFields.Add(new OrderField() { FieldName = OpenExpression.FieldName, Descending = OpenExpression.Descending });
        }
        protected virtual void VisitWhereIn(WhereInOpenExpression OpenExpression)
        {
            IWhereOpenExpression exp = new FalseOpenExpression();
            foreach (var value in OpenExpression.Values)
            {
                exp = new OrElseOpenExpression(exp, new WhereEqualsOpenExpression(null, OpenExpression.FieldName, value));
            }
            this.VisitWhere(exp);
        }
        protected virtual void VisitWhereNotIn(WhereNotInOpenExpression OpenExpression)
        {
            IWhereOpenExpression exp = new TrueOpenExpression();
            foreach (var value in OpenExpression.Values)
            {
                exp = new AndAlsoOpenExpression(exp, new WhereNotEqualsOpenExpression(null, OpenExpression.FieldName, value));
            }
            this.VisitWhere(exp);
        }

        protected virtual void VisitCall(CallOpenExpression OpenExpression)
        {
            if (OpenExpression.CallType == CallType.First || OpenExpression.CallType == CallType.FirstOrDefault || OpenExpression.CallType == CallType.Last || OpenExpression.CallType == CallType.LastOrDefault)
            {
                this.Top = 1;
            }
        }

        //protected abstract void VisitWhereCategory(WhereCategoryOpenExpression OpenExpression);

        protected abstract void VisitWhereBetweenOrEqual(WhereBetweenOrEqualOpenExpression OpenExpression);
        protected abstract void VisitWhereBetween(WhereBetweenOpenExpression OpenExpression);
        protected abstract void VisitWhereContains(WhereContainsOpenExpression OpenExpression);
        protected abstract void VisitWhereEndsWith(WhereEndsWithOpenExpression OpenExpression);
        protected virtual void VisitWhereEquals(WhereEqualsOpenExpression OpenExpression)
        {
            this.Filters.Add(new OpenFilter()
            {
                ConditionType = ExpressionType.Equal,
                Name = OpenExpression.FieldName,
                Value = OpenExpression.Value.ToString(),
            });
        }
        protected virtual void VisitWhereClause(WhereClauseOpenExpression OpenExpression)
        {

        }
        protected abstract void VisitWhereGreaterThan(WhereGreaterThanOpenExpression OpenExpression);
        protected abstract void VisitWhereGreaterThanOrEqual(WhereGreaterThanOrEqualOpenExpression OpenExpression);

        protected abstract void VisitWhereLessThan(WhereLessThanOpenExpression OpenExpression);
        protected abstract void VisitWhereLessThanOrEqual(WhereLessThanOrEqualOpenExpression OpenExpression);

        protected abstract void VisitWhereStartsWith(WhereStartsWithOpenExpression OpenExpression);
        protected abstract void VisitWhereNotEquals(WhereNotEqualsOpenExpression OpenExpression);

        protected abstract void VisitAndAlso(AndAlsoOpenExpression OpenExpression);
        protected abstract void VisitOrElse(OrElseOpenExpression OpenExpression);
        protected abstract void VisitFalse(FalseOpenExpression OpenExpression);
        protected abstract void VisitTrue(TrueOpenExpression OpenExpression);

        protected virtual void VisitWhere(IWhereOpenExpression OpenExpression)
        {
            if (OpenExpression is TrueOpenExpression)
            {
                VisitTrue((TrueOpenExpression)OpenExpression);
            }
            else if (OpenExpression is FalseOpenExpression)
            {
                VisitFalse((FalseOpenExpression)OpenExpression);
            }
            if (OpenExpression is WhereBetweenOrEqualOpenExpression)
            {
                var whereBetweenOpenExpression = (WhereBetweenOrEqualOpenExpression)OpenExpression;
                VisitWhereBetweenOrEqual(whereBetweenOpenExpression);
            }
            else if (OpenExpression is WhereBetweenOpenExpression)
            {
                var whereBetweenOpenExpression = (WhereBetweenOpenExpression)OpenExpression;
                VisitWhereBetween(whereBetweenOpenExpression);

            }
            else if (OpenExpression is WhereContainsOpenExpression)
            {
                var whereContainsOpenExpression = (WhereContainsOpenExpression)OpenExpression;
                VisitWhereContains(whereContainsOpenExpression);
            }
            else if (OpenExpression is WhereEndsWithOpenExpression)
            {
                var whereEndsWithOpenExpression = (WhereEndsWithOpenExpression)OpenExpression;
                VisitWhereEndsWith(whereEndsWithOpenExpression);
            }
            else if (OpenExpression is WhereEqualsOpenExpression)
            {
                var whereEqualsOpenExpression = (WhereEqualsOpenExpression)OpenExpression;
                VisitWhereEquals(whereEqualsOpenExpression);
            }
            else if (OpenExpression is WhereClauseOpenExpression)
            {
                var whereOpenExpression = (WhereClauseOpenExpression)OpenExpression;
                VisitWhereClause(whereOpenExpression);
            }
            else if (OpenExpression is WhereGreaterThanOpenExpression)
            {
                var whereGreaterThan = (WhereGreaterThanOpenExpression)OpenExpression;
                VisitWhereGreaterThan(whereGreaterThan);
            }
            else if (OpenExpression is WhereGreaterThanOrEqualOpenExpression)
            {
                var whereGreaterThenOrEquals = (WhereGreaterThanOrEqualOpenExpression)OpenExpression;
                VisitWhereGreaterThanOrEqual(whereGreaterThenOrEquals);
            }
            else if (OpenExpression is WhereLessThanOpenExpression)
            {
                var whereLessThan = (WhereLessThanOpenExpression)OpenExpression;
                VisitWhereLessThan(whereLessThan);
            }
            else if (OpenExpression is WhereLessThanOrEqualOpenExpression)
            {
                var whereLessThanOrEqual = (WhereLessThanOrEqualOpenExpression)OpenExpression;
                VisitWhereLessThanOrEqual(whereLessThanOrEqual);
            }
            else if (OpenExpression is WhereNotEqualsOpenExpression)
            {
                VisitWhereNotEquals((WhereNotEqualsOpenExpression)OpenExpression);
            }
            else if (OpenExpression is WhereStartsWithOpenExpression)
            {
                var whereStartWith = (WhereStartsWithOpenExpression)OpenExpression;
                VisitWhereStartsWith(whereStartWith);
            }
            else if (OpenExpression is WhereInOpenExpression)
            {
                var whereInOpenExpression = (WhereInOpenExpression)OpenExpression;
                VisitWhereIn(whereInOpenExpression);
            }
            else if (OpenExpression is WhereNotInOpenExpression)
            {
                var whereNotInOpenExpression = (WhereNotInOpenExpression)OpenExpression;
                VisitWhereNotIn(whereNotInOpenExpression);
            }
            else if (OpenExpression is OrElseOpenExpression)
            {
                VisitOrElse((OrElseOpenExpression)OpenExpression);
            }
            else if (OpenExpression is AndAlsoOpenExpression)
            {
                VisitAndAlso((AndAlsoOpenExpression)OpenExpression);
            }
        }
    }

}
