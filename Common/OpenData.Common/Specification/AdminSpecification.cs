using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Specification
{
    public class AdminUser
    {
        private ISpecification<AdminUser> hasRightSpecification = new HasRightSpecification();
        private ISpecification<AdminUser> isActiveSpecification = new IsActiveSpecification();
        public string Roles { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public bool HasRight()
        {
            return this.isActiveSpecification.And(this.hasRightSpecification).IsSatisfiedBy(this);
        }
        public ISpecification<AdminUser> GetSpecification()
        {
            return this.isActiveSpecification.And(this.hasRightSpecification);
        }
    }

    public class HasRightSpecification : CompositeSpecification<AdminUser>
    {
        public override bool IsSatisfiedBy(AdminUser condidate)
        {
            return condidate.Roles.Contains("admin");
        }
    }

    public class IsActiveSpecification : CompositeSpecification<AdminUser>
    {
        public override bool IsSatisfiedBy(AdminUser condidate)
        {
            return condidate.IsActive;
        }
    }

    public class EqualsSpecification<T> : CompositeSpecification<T>
    {
        Func<T, bool> condidate;
        public EqualsSpecification(Func<T, bool> condidate)
        {
            this.condidate = condidate;
        }
        public override bool IsSatisfiedBy(T condidate)
        {
            return this.condidate(condidate);
        }
    }
}
