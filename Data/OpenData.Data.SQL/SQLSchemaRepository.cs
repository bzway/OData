using Bzway.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Bzway.Data.SQLServer
{
    public class SchemaRepository : ISchemaRepository
    {
        readonly string connectionString;
        readonly string tableName;
        public SchemaRepository(string connectionString)
        {
            this.connectionString = connectionString;
            this.tableName = "SYSTEMSCHEMA";
        }

        public IEnumerable<string> Query()
        {
            string sql = string.Format("select * from [{0}]", this.tableName);
            SqlConnection conn = new SqlConnection(connectionString);
            List<string> list = new List<string>();
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var entityName = reader["ProviderName"].ToString();
                    list.Add(entityName);
                }
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

            }
            return list;
        }

        public void Insert(string schema)
        {
            string sql = string.Format("insert into [{0}](ProviderName) values{'{1}'}", this.tableName, schema);
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public void Delete(string schema)
        {
            var entityName = schema;
            foreach (var item in this.Columns(entityName).Query())
            {
                this.Columns(entityName).Delete(item);
            }
            string sql = string.Format("delete from [{0}] where ProviderName='{1}'", this.tableName, entityName);
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public IColumnRepository Columns(string entityName)
        {
            return new ColumnRepository(this.connectionString, entityName);
        }
    }
    public class ColumnRepository : IColumnRepository
    {
        readonly string connectionString;
        readonly string entityName;
        readonly string tableName;
        public ColumnRepository(string connectionString, string entityName)
        {
            this.connectionString = connectionString;
            this.entityName = entityName;
            this.tableName = "SYSTEMSCHEMADETAIL";
        }

        public void Insert(Column column)
        {
            string sql = string.Format("insert into [{0}] ()", this.tableName);

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

            }
        }

        public IEnumerable<Column> Query()
        {
            string sql = string.Format("select * from [{0}] where ProviderName ='{1}'", this.tableName, this.entityName);

            SqlConnection conn = new SqlConnection(connectionString);
            List<Column> list = new List<Column>();

            try
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Column()
                    {
                        Name = reader["ProviderName"].ToString(),
                        AllowNull = (bool)reader["AllowNull"],
                        ControlType = reader["ControlType"].ToString(),
                        DefaultValue = reader["DefaultValue"].ToString(),
                        Indexable = (bool)reader["Indexable"],
                        IsSystemField = (bool)reader["IsSystemField"],
                        Label = reader["Label"].ToString(),
                        Length = (int)reader["Length"],
                        Modifiable = (bool)reader["Modifiable"],
                        Order = (int)reader["Order"],
                        ShowInGrid = (bool)reader["ShowInGrid"],
                        Tooltip = reader["Tooltip"].ToString(),
                    });
                }

            }
            catch (SqlException ex)
            {

            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

            }
            return list;
        }

        public void Delete(Column column)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = string.Format("delete from [{0}] where ProviderName = '{1}'", this.tableName, this.entityName);
            try
            {

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

            }
        }
    }
}