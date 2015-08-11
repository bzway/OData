
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OpenData.Framework
{
    public class OpenDatabase : IDisposable
    {
        public static IProvider Provider { get; set; }
        private string connectionString;
        public OpenDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IOpenRepository<OpenEntity> Entity(string entityName)
        {
            var schemaRepository = Provider.GetSchemaRepoistory(connectionString, entityName);
            return Provider.GetDataRepoistory<OpenEntity>(connectionString, entityName, schemaRepository);
        }
        public IOpenRepository<T> Entity<T>() where T : OpenEntity
        {
            var type = typeof(T);
            var entityName = type.Name;
            var schemaRepository = Provider.GetSchemaRepoistory(connectionString, entityName);
            return Provider.GetDataRepoistory<T>(connectionString, entityName, schemaRepository);
        }
        public void Dispose()
        {

        }
    }
    public interface IProvider
    {
        IOpenRepository<T> GetDataRepoistory<T>(string connectionString, string entityName, ISchemaRepository schemaRepository) where T : OpenEntity;
        ISchemaRepository GetSchemaRepoistory(string connectionString, string entityName);
    }

    public class MyClass
    {
        Func<OpenEntity, bool> ee = m => { return m.UUID == ""; };

        public MyClass()
        {
        }
    }
}
