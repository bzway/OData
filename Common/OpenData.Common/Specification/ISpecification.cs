using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T condidate);
        ISpecification<T> And(ISpecification<T> Other);
        ISpecification<T> Or(ISpecification<T> Other);
        ISpecification<T> Not();
    }


}
