
using OpenData.Framework;
using System.Data.Entity;

namespace OpenData.WebApp
{
    public class MyApplicationDbContext : DbContext
    {
        public MyApplicationDbContext()
            : this("DefaultConnection")
        { }

        public MyApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new PersonMap());
            //modelBuilder.Configurations.Add(new AccountMap());
            //modelBuilder.Configurations.Add(new CityMap());
            //modelBuilder.Configurations.Add(new ProvinceMap());
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<OpenEntityUser> Users { get; set; }
    }

    public class OpenEntityUser : OpenEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}