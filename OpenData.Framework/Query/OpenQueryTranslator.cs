
using OpenData.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpenData.Framework.Query;
using OpenData.Framework.Query.OpenExpressions;

namespace QueryTranslator
{
    public class OpenQueryTranslator : OpenData.Framework.OpenExpressionVisitor
    {
        protected override void VisitSkip(SkipOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitTake(TakeOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

     

        protected override void VisitWhereBetweenOrEqual(WhereBetweenOrEqualOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereBetween(WhereBetweenOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereContains(WhereContainsOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereEndsWith(WhereEndsWithOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }



        protected override void VisitWhereGreaterThan(WhereGreaterThanOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereGreaterThanOrEqual(WhereGreaterThanOrEqualOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereLessThan(WhereLessThanOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereStartsWith(WhereStartsWithOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitWhereNotEquals(WhereNotEqualsOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitAndAlso(AndAlsoOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitOrElse(OrElseOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitFalse(FalseOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }

        protected override void VisitTrue(TrueOpenExpression OpenExpression)
        {
            throw new NotImplementedException();
        }
    }
}