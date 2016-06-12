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
    public class BzwayIdentity : IIdentity
    {
        public string ID { get; set; }
        public string Secret { get; set; }
        public string Roles { get; set; }

        public BzwayIdentity() { }
        public BzwayIdentity(string data) { Parse(data); }

        public BzwayIdentity Parse(string data)
        {
            try
            {
                data = Cryptor.DecryptAES(data, "BzwayIdentity");
                string[] ds = data.Split('|');
                this.ID = ds[0];
                this.Secret = ds[1];
                this.Roles = ds[2];
            }
            catch//(Exception ex)
            {

            }
            return this;
        }
        string s = "|";
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID);
            sb.Append(s);
            sb.Append(Secret);
            sb.Append(s);
            sb.Append(Roles);
            return Cryptor.EncryptAES(sb.ToString(), "BzwayIdentity");
        }
        public string AuthenticationType
        {
            get { return "Form"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.ID); }
        }

        /// <summary>
        /// User Name, can retrieve user info from system db
        /// </summary>
        public string Name
        {
            get { return this.ID; }
        }
    }
}