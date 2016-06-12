using OpenData.Data;

namespace OpenData.Framework.Entity
{
    public class PageBlock : BaseEntity
    {
        public string Name { get; set; }
        public string PageId { get; set; }
        public string ViewId { get; set; }
        public int OrderBy { get; set; }
    }
}