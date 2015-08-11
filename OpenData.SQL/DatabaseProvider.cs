
using OpenData.Framework;
namespace OpenData.SQL
{
    public class SQLDatabaseProvider : IProvider
    {




        public IOpenRepository<T> GetDataRepoistory<T>(string connectionString, string entityName, ISchemaRepository schemaRepository) where T : OpenEntity
        {
            return new SQLRepository<T>(connectionString, entityName);
        }


        public ISchemaRepository GetSchemaRepoistory(string connectionString, string entityName)
        {
            return new SQLSchemaRepository(connectionString, entityName);

        }
    }
}