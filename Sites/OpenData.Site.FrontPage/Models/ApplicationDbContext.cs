using OpenData.Data.Core;
using System;
using System.Data.Entity;

namespace OpenData.Site.FrontPage.Models
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }
    }
    /// <summary>
    /// log4net table
    /// </summary>
    public class SystemLog : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}