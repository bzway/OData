
using Bzway.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bzway.Data
{
    public class FileSchemaRepository : ISchemaRepository
    {
        readonly string Dir;
        public FileSchemaRepository(string connectionString, string databaseName)
        {
            var BaseDir = new Bzway.Website.Common.BaseDir();
            this.Dir = System.IO.Path.Combine(BaseDir.AppDataPhysicalPath, connectionString, databaseName);
            if (!Directory.Exists(this.Dir))
            {
                Directory.CreateDirectory(this.Dir);
            }
        }

        public IEnumerable<string> Query()
        {
            var filePath = this.Dir + "\\Schema.config";
            var list = SerializationHelper.DeserializeObjectJson<List<string>>(File.ReadAllText(filePath));
            return list;
        }

        public void Insert(string Schema)
        {
            var list = this.Query().ToList();
            list.Add(Schema);
            var filePath = this.Dir + "\\Schema.config";
            File.WriteAllText(filePath, SerializationHelper.SerializeObjectToJson(list));
        }
        public void Delete(string Schema)
        {
            var list = this.Query().ToList();
            list.Remove(Schema);
            var filePath = this.Dir + "\\Schema.config";
            File.WriteAllText(filePath, SerializationHelper.SerializeObjectToJson(list));
        }

        public IColumnRepository Columns(string entityName)
        {
            return new FileColumnRepository(this.Dir, entityName);
        }
    }
    public class FileColumnRepository : Schema, IColumnRepository
    {
        static Dictionary<string, IList<Column>> dict = new Dictionary<string, IList<Column>>();

        readonly string Dir;
        readonly string entityName;
        public FileColumnRepository(string dir, string entityName, IEnumerable<Column> columns = null)
            : base(entityName, columns)
        {
            this.Dir = dir;
            this.entityName = entityName;
            if (dict.ContainsKey(entityName))
            {
                foreach (var item in dict[entityName])
                {
                    this.AddColumn(item);
                }
                return;
            }

            if (columns != null)
            {
                foreach (var item in columns)
                {
                    this.AddColumn(item);
                }
            }
            var filePath = this.Dir + string.Format("\\{0}\\setting.config", entityName);
            var list = SerializationHelper.DeserializeObjectJson<List<Column>>(File.ReadAllText(filePath));

            foreach (var column in list)
            {
                this.AddColumn(column);
            }
            dict[entityName] = this.AllColumns;
        }

        public IEnumerable<Column> Query()
        {
            return dict[entityName];
        }

        public void Insert(Column column)
        {
            if (this.AddColumn(column))
            {
                dict[entityName] = this.AllColumns;
                var filePath = this.Dir + string.Format("\\{0}\\setting.config", entityName);
                File.WriteAllText(filePath, SerializationHelper.SerializeObjectToJson(this.AllColumns));
            }
        }

        public void Delete(Column column)
        {
            if (base.RemoveColumn(column) > 0)
            {
                dict[entityName] = this.AllColumns;
                var filePath = this.Dir + string.Format("\\{0}\\setting.config", entityName);
                File.WriteAllText(filePath, SerializationHelper.SerializeObjectToJson(this.AllColumns));
            }
        }
    }
}