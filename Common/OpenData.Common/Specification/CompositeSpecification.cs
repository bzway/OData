using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Specification
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T condidate);
        public ISpecification<T> And(ISpecification<T> Other)
        {
            return new AndSpecification<T>(this, Other);
        }
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
        public ISpecification<T> Or(ISpecification<T> Other)
        {
            return new OrSpecification<T>(this, Other);
        }
    }
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> leftSpecification;
        private ISpecification<T> rightSpecification;
        public AndSpecification(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
        {
            this.leftSpecification = leftSpecification;
            this.rightSpecification = rightSpecification;
        }
        public override bool IsSatisfiedBy(T condidate)
        {
            return leftSpecification.IsSatisfiedBy(condidate) && rightSpecification.IsSatisfiedBy(condidate);
        }
    }
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> leftSpecification;

        public NotSpecification(ISpecification<T> leftSpecification)
        {
            this.leftSpecification = leftSpecification;
        }
        public override bool IsSatisfiedBy(T condidate)
        {
            return !leftSpecification.IsSatisfiedBy(condidate);
        }
    }
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> leftSpecification;
        private ISpecification<T> rightSpecification;

        public OrSpecification(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
        {
            this.leftSpecification = leftSpecification;
            this.rightSpecification = rightSpecification;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return leftSpecification.IsSatisfiedBy(candidate) || rightSpecification.IsSatisfiedBy(candidate);
        }
    }
}
