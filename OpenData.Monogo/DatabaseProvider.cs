using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Shared;
using MongoDB.Bson;
using QueryTranslator; 
using Microsoft.Owin;
using System.Web;
[assembly: PreApplicationStartMethod(typeof(OpenData.Framework.DatabaseProvider), "Configuration")]
namespace OpenData.Framework
{

    public class DatabaseProvider : IProvider
    {
        public static void Configuration()
        {
            OpenDatabase.Provider = new DatabaseProvider();
        }
        public ISchemaRepository GetSchemaRepoistory(string connectionString, string entityName)
        {
            return new MongoSchemaRepository(connectionString, entityName);
        }

        public IOpenRepository<T> GetDataRepoistory<T>(string connectionString, string entityName, ISchemaRepository schemaRepository) where T : OpenEntity
        {
            return new MongoRepository<T>(connectionString, entityName, schemaRepository);
        }
    }
}