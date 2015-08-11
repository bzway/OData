
using OpenData.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;



namespace OpenData.SQL
{
    public class SQLRepository<T> : IOpenRepository<T> where T : OpenEntity
    {
        string connectionString;
        string entityName;
        Schema schema;
        public SQLRepository(string connectionString, string entityName)
        {
            this.connectionString = connectionString;
            this.entityName = entityName;
        }

        public virtual void Insert(T newData)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = string.Format("insert into [{0}]", entityName);
            foreach (var item in schema.AllColumns)
            {
                sql += string.Format("[{0}]", item.Name);
            }
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public virtual void Update(T newData, T oldData)
        {
            throw new System.NotImplementedException();
        }


        public ISchemaRepository Schema
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void Delete(T oldData)
        {
            throw new System.NotImplementedException();
        }

        public object Execute(IOpenQuery<T> query)
        {
            throw new System.NotImplementedException();
        }

        public IOpenQuery<T> Query()
        {
            throw new System.NotImplementedException();
        }
    }
}