using OpenData.Data.Core;

namespace OpenData.Framework.Core.Entity
{
    public class App : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Domains { get; set; }
        public string TimeZone { get; set; }
        public string ProviderName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}