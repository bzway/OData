using OpenData.Data;

namespace OpenData.Site.Entity
{

    public class District : BaseEntity
    {
        public string CityID { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}