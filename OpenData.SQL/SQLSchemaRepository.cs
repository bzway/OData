using OpenData.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OpenData.SQL
{
    public class SQLSchemaRepository : ISchemaRepository
    {
        string connectionString;
        public string Name { get; set; }
        readonly string dbName = "SYSTEMSCHEMA";
        public SQLSchemaRepository(string connectionString, string entityName)
        {
            this.connectionString = connectionString;
            this.Name = entityName;
        }
        Schema schema;
        public SQLSchemaRepository()
        {
            string sql = string.Format("select * from [{0}] where Name='{1}'", dbName, Name);
            SqlConnection conn = new SqlConnection(connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            var reader = cmd.ExecuteReader();
            List<Column> list = new List<Column>();
            while (reader.Read())
            {
                list.Add(new Column()
                {
                    Name = reader["Name"].ToString(),
                    Label = reader["Label"].ToString(),
                    ControlType = reader["ControlType"].ToString(),
                    AllowNull = (bool)reader["AllowNull"],
                    Length = (int)reader["Length"],
                    Order = (int)reader["Order"],
                    Modifiable = (bool)reader["Modifiable"],
                    ShowInGrid = (bool)reader["ShowInGrid"],
                    Tooltip = reader["Tooltip"].ToString(),
                    DefaultValue = reader["DefaultValue"].ToString(),
                    Summarize = (bool)reader["Summarize"],
                    Indexable = (bool)reader["Indexable"],
                    IsSystemField = (bool)reader["IsSystemField"],
                    SelectionFolder = reader[""].ToString(),
                });
            }
            this.schema = new Schema(Name, list);
        }

        public void Insert(Column column)
        {
            if (this.schema.AddColumn(column))
            {
                /*
                 如果新增加字段需要在SYSTEMSCHEMA表中增加记录，并且实体表的字段也需要增加
                 */
                string sql = string.Format("insert into [{0}]", Name);
                foreach (var item in schema.AllColumns)
                {
                    sql += string.Format("[{0}]", item.Name);
                }
                SqlConnection conn = new SqlConnection(connectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Column> Query()
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Column column)
        {
            throw new System.NotImplementedException();
        }
    }
}