using System.Security.Principal;


namespace OpenData.Framework.Common
{
     public class BzwayPrincipal : IPrincipal
    {
        public BzwayPrincipal(string data)
        {
            this.BzwayIdentity = new BzwayIdentity(data);
        }
        private BzwayIdentity BzwayIdentity { get; set; }
        public IIdentity Identity
        {
            get
            {
                return this.BzwayIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            return this.BzwayIdentity.Roles.Contains(role);
        }
        public override string ToString()
        {
            return this.BzwayIdentity.Name;
        }
    }
     
}