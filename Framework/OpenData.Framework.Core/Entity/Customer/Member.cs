using OpenData.Data.Core;
namespace OpenData.Framework.Core.Entity
{
    public class Member : BaseEntity
    {
        public string UserID { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public GenderType Gender { get; set; }
    }
}
