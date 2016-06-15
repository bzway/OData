using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using OpenData.Data.Core;
using Autofac;

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

        IContainer container = null;

        [TestMethod]
        public void TestMethod2()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<test>().As<Itest>();
            container = builder.Build();
            container.Resolve<Itest>().test1();

            builder = new ContainerBuilder();
            builder.RegisterType<exe>().As<Iexe>();
            builder.Update(container);
            container.Resolve<Itest>().test1();
            container.Resolve<Iexe>().exec();
        }
    }

    public class exe : Iexe
    {
        public void exec()
        {
            Console.WriteLine("exec");
        }
    }
    public class Todo : Itest
    {
        public string Name
        {
            get; set;
        }

        public string UUID
        {
            get; set;
        }

        public void test1()
        {
            Console.WriteLine("todo");
        }
    }
    public class test : BaseEntity, Itest
    {
        public string UUID { get; set; }
        public string Name { get; set; }

        public void test1()
        {
            Console.WriteLine("test1");
        }
    }
}