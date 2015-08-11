using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenData.Framework
{
    public class MongoSchemaRepository : ISchemaRepository
    {
        string connectionString;
        public string Name { get; set; }
        readonly string dbName = "SYSTEMSCHEMA";
        public MongoSchemaRepository(string connectionString, string entityName)
        {
            this.connectionString = connectionString;
            this.Name = entityName;


        }

        public IEnumerable<Column> Query()
        {

            var client = new MongoClient();
            var db = client.GetDatabase(this.connectionString);
            var filter = Builders<BsonDocument>.Filter.Eq("EntityName", this.Name);
            var sort = Builders<BsonDocument>.Sort.Ascending("_id");

            var collection = db.GetCollection<BsonDocument>(this.dbName);
            var result = collection.Find(filter).Sort(sort).ToListAsync().Result;


            List<Column> list = new List<Column>();

            foreach (var column in result)
            {

                var entity = new Column()
                {
                    Name = column["Name"].ToString(),
                    IsSystemField = column["IsSystemField"].ToBoolean(),
                    Indexable = column["Indexable"].ToBoolean(),
                    Tooltip = column["Tooltip"].ToString(),
                    Summarize = column["Summarize"].ToBoolean(),
                    ShowInGrid = column["ShowInGrid"].ToBoolean(),
                    AllowNull = column["AllowNull"].ToBoolean(),
                    Order = column["Order"].ToInt32(),
                    DefaultValue = column["DefaultValue"].ToString(),
                    Label = column["Label"].ToString(),
                    SelectionFolder = column["SelectionFolder"].ToString(),
                    ControlType = column["ControlType"].ToString(),
                    //CustomSettings = column["CustomSettings"],
                    Length = column["Length"].ToInt32(),
                    Modifiable = column["Modifiable"].ToBoolean(),

                };
                list.Add(entity);
            }
            return list;
        }

        public void Insert(Column column)
        {
            if (this.Query().Where(m => m.Name == column.Name).FirstOrDefault() != null)
            {
                return;
            }
            var client = new MongoClient();
            var db = client.GetDatabase(this.connectionString);
            var collection = db.GetCollection<BsonDocument>(this.dbName);
            var insert = new BsonDocument();
            insert.Add("EntityName", this.Name);

            insert.Add("Name", column.Name);
            insert.Add("IsSystemField", column.IsSystemField);
            insert.Add("Indexable", column.Indexable);
            insert.Add("Tooltip", column.Tooltip);
            insert.Add("Summarize", column.Summarize);
            insert.Add("ShowInGrid", column.ShowInGrid);
            insert.Add("AllowNull", column.AllowNull);
            insert.Add("Order", column.Order);
            insert.Add("DefaultValue", column.DefaultValue);
            insert.Add("Label", column.Label);
            insert.Add("SelectionFolder", column.SelectionFolder);
            insert.Add("ControlType", column.ControlType);
            //insert.Add("CustomSettings",column.CustomSettings);
            insert.Add("Length", column.Length);
            insert.Add("Modifiable", column.Modifiable);

            var result = collection.InsertOneAsync(insert);
        }

        public void Delete(Column column)
        {
            var client = new MongoClient();
            var db = client.GetDatabase(this.connectionString);
            var collection = db.GetCollection<BsonDocument>(this.dbName);
            var filter = Builders<BsonDocument>.Filter.Eq("Name", column.Name);
            filter &= Builders<BsonDocument>.Filter.Eq("EntityName", this.Name);
            var result = collection.DeleteOneAsync(filter);
        }
    }
}