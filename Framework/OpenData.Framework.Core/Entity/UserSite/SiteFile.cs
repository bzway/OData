using OpenData.Data.Core;

namespace OpenData.Framework.Core.Entity
{

    public class SiteFile : BaseEntity
    {

    }
    public class SiteNavigation : BaseEntity
    {
        public string ParantId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// For Virtual Path
        /// </summary>
        public string Name { get; set; }
    }
}