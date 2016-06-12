using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Bzway.Data;
namespace DataTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = OpenDatabase.GetDatabase("SQLServer"))
            {
                db.Entity<test>().Insert(new test() { Name = "test", UUID = "test" });
                var entity = db.Entity<test>().Query().Where(m => m.UUID, "test", CompareType.Equal).First();
                Assert.IsNotNull(entity);
                db.Entity<test>().Update(new test() { Name = "updated", UUID = "test" });
                entity = db.Entity<test>().Query().Where(m => m.Name, "updated", CompareType.Equal).First();
                Assert.IsNotNull(entity);
                db.Entity<test>().Delete("test");
                entity = db.Entity<test>().Query().Where(m => m.Name, "updated", CompareType.Equal).First();
                Assert.IsNull(entity);
                var list = db.Entity<test>().Query().ToList();
            }
        }
    }

    public class test : BaseEntity
    {
        public string UUID { get; set; }
        public string Name { get; set; }

    }
}