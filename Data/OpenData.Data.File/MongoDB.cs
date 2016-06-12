using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Shared;
using MongoDB.Bson;
using Microsoft.Owin;
using System.Web;
[assembly: PreApplicationStartMethod(typeof(Bzway.DynamicData.Framework.DatabaseProvider), "Configuration")]
namespace Bzway.DynamicData.Framework
{
    public class MongoDB : OpenDatabase
    {
        public MongoDB(string connectionString)
            : base(new DatabaseProvider(), connectionString)
        {

        }
    }
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

        public IOpenRepository<T> GetDataRepoistory<T>(ISchemaRepository schemaRepository) where T : OpenEntity
        {
            return new MongoRepository<T>(schemaRepository);
        }
    }
}