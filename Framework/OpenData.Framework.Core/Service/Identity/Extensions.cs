using System.Security.Principal;

namespace OpenData.Security
{
    public static class Extensions
    {
        public static BzwayIdentity BzwayIdentity(this IPrincipal p)
        {
            return p.Identity as BzwayIdentity;
        }

    }
}