using OpenData.Utility;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace OpenData.Security
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