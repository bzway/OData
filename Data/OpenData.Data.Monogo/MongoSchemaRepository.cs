using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
//using FluentAssertions;

namespace Bzway.Data.Mongo
{
    public class MongoSchemaRepository : ISchemaRepository
    {
        readonly string connectionString;
        readonly string datebaseName;
        readonly string collectionName;
        public MongoSchemaRepository(string connectionString, string databaseName)
        {
            this.connectionString = connectionString;
            this.datebaseName = databaseName;
            this.collectionName = "SYSTEMSCHEMA";
        }

        public IEnumerable<string> Query()
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Ascending("ProviderName");

            var result = collection.Find(filter).Sort(sort).ToListAsync().GetAwaiter().GetResult();
            List<string> list = new List<string>();
            foreach (var column in result)
            {
                var name = column["ProviderName"].ToString();
                list.Add(name);
            }
            return list;
        }

        public void Insert(string Schema)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("ProviderName", Schema);
            var count = collection.Find(filter).CountAsync().GetAwaiter().GetResult();
            if (count == 0)
            {
                var document = new BsonDocument { 
                                                    {"ProviderName", Schema  },
                                                };
                collection.InsertOneAsync(document);
            }
        }
        public void Delete(string Schema)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("ProviderName", Schema);
            foreach (var item in this.Columns(Schema).Query())
            {
                this.Columns(Schema).Delete(item);
            }
            collection.DeleteManyAsync(filter);
        }

        public IColumnRepository Columns(string entityName)
        {
            return new MongoColumnRepository(this.connectionString, this.datebaseName, entityName);
        }
    }
    public class MongoColumnRepository : Schema, IColumnRepository
    {
        static Dictionary<string, IList<Column>> dict = new Dictionary<string, IList<Column>>();
        readonly string connectionString;
        readonly string datebaseName;
        readonly string collectionName;
        readonly string entityName;
        public MongoColumnRepository(string connectionString, string databaseName, string entityName, IEnumerable<Column> columns = null)
            : base(entityName, columns)
        {
            this.connectionString = connectionString;
            this.datebaseName = databaseName;
            this.collectionName = "SYSTEMSCHEMADETAIL";
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
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("EntityName", this.entityName);
            var sort = Builders<BsonDocument>.Sort.Ascending("Order").Ascending("CreatedOn");

            var result = collection.Find(filter).Sort(sort).ToListAsync().GetAwaiter().GetResult();

            foreach (var column in result)
            {
                var entity = new Column()
                {
                    Name = column["ProviderName"].ToString(),
                    IsSystemField = column["IsSystemField"].ToBoolean(),
                    Indexable = column["Indexable"].ToBoolean(),
                    Tooltip = column["Tooltip"].ToString(),
                    ShowInGrid = column["ShowInGrid"].ToBoolean(),
                    AllowNull = column["AllowNull"].ToBoolean(),
                    Order = column["Order"].ToInt32(),
                    DefaultValue = column["DefaultValue"].ToString(),
                    Label = column["Label"].ToString(),
                    ControlType = column["ControlType"].ToString(),
                    Length = column["Length"].ToInt32(),
                    Modifiable = column["Modifiable"].ToBoolean(),

                };
                this.AddColumn(entity);
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
                var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);

                if (null != collection.Find(m => m["ProviderName"] == column.Name).FirstOrDefaultAsync().GetAwaiter().GetResult())
                {
                    return;
                }

                var insert = new BsonDocument();
                insert.Add("EntityName", BsonValue.Create(this.entityName));
                insert.Add("ProviderName", BsonValue.Create(column.Name));
                insert.Add("IsSystemField", BsonValue.Create(column.IsSystemField));
                insert.Add("Indexable", BsonValue.Create(column.Indexable));
                insert.Add("Tooltip", BsonValue.Create(column.Tooltip));
                insert.Add("ShowInGrid", BsonValue.Create(column.ShowInGrid));
                insert.Add("AllowNull", BsonValue.Create(column.AllowNull));
                insert.Add("Order", BsonValue.Create(column.Order));
                insert.Add("DefaultValue", BsonValue.Create(column.DefaultValue));
                insert.Add("Label", BsonValue.Create(column.Label));
                insert.Add("ControlType", BsonValue.Create(column.ControlType));
                insert.Add("Length", BsonValue.Create(column.Length));
                insert.Add("Modifiable", BsonValue.Create(column.Modifiable));
                var result = collection.InsertOneAsync(insert);
                dict[entityName] = this.AllColumns;
            }
        }

        public void Delete(Column column)
        {
            if (base.RemoveColumn(column) > 0)
            {
                var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("ProviderName", column.Name);
                filter &= Builders<BsonDocument>.Filter.Eq("EntityName", this.entityName);
                var result = collection.DeleteOneAsync(filter);
                dict[entityName] = this.AllColumns;
            }
        }
    }
}