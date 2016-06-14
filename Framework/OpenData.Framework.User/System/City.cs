using OpenData.Data;

namespace OpenData.Site.Entity
{

    public class City : BaseEntity
    {
        public string ProinceID { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}