using MongoDB.Bson;
using MongoDB.Driver;
using QueryTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OpenData.Framework
{

    public class MongoRepository<T> : IOpenRepository<T> where T : OpenEntity
    {

        public ISchemaRepository Schema
        {
            get;
            set;
        }
        string connectionString;
        public MongoRepository(string connectionString, string entityName, ISchemaRepository schemaRepository)
        {
            this.connectionString = connectionString;
            this.Schema = schemaRepository;
        }

        public virtual void Insert(T newData)
        {
            var client = new MongoClient();
            var db = client.GetDatabase(this.connectionString);
            var collection = db.GetCollection<BsonDocument>(this.Schema.Name);
            var insert = new BsonDocument();
            foreach (var item in this.Schema.Query())
            {
                if (newData.ContainsKey(item.Name))
                {
                    insert.Add(item.Name, newData[item.Name].ToString());
                }
            }
            var result = collection.InsertOneAsync(insert);
        }

        public virtual void Update(T newData, T oldData)
        {
            var client = new MongoClient();
            var db = client.GetDatabase(this.connectionString);

            var collection = db.GetCollection<BsonDocument>(this.Schema.Name);
            var filter = Builders<BsonDocument>.Filter.Eq("UUID", newData.UUID);

            var update = Builders<BsonDocument>.Update.CurrentDate("UpdatedOn");
            foreach (var item in this.Schema.Query())
            {
                update.Set(item.Name, oldData[item.Name]);
            }


            var result = collection.UpdateOneAsync(filter, update);
        }

        public virtual void Delete(T oldData)
        {
            throw new NotImplementedException();
        }

        public virtual object Execute(IOpenQuery<T> query)
        {
            OpenQueryTranslator translator = new OpenQueryTranslator();
            translator.Visite(query.OpenExpression);

            IMongoClient client = new MongoClient();
            var db = client.GetDatabase(this.connectionString);
            var collection = db.GetCollection<BsonDocument>(Schema.Name);

            FilterDefinition<BsonDocument> filter = null;
            foreach (var item in translator.Filters)
            {
                switch (item.ConditionType)
                {
                    case ExpressionType.Equal:
                        if (filter == null)
                        {
                            filter = Builders<BsonDocument>.Filter.Eq(item.Name, item.Value);
                        }
                        else
                        {
                            filter &= Builders<BsonDocument>.Filter.Eq(item.Name, item.Value);
                        }
                        break;
                    case ExpressionType.NotEqual:
                        if (filter == null)
                        {
                            filter = Builders<BsonDocument>.Filter.Ne(item.Name, item.Value);
                        }
                        else
                        {
                            filter &= Builders<BsonDocument>.Filter.Ne(item.Name, item.Value);
                        }
                        break;
                    default:
                        throw new Exception(item.ConditionType.ToString());
                }
            }


            var sort = Builders<BsonDocument>.Sort.Ascending("Name").Ascending("_id");


            var result =   collection.Find(filter).Sort(sort).ToListAsync().GetAwaiter().GetResult();

            List<OpenEntity> list = new List<OpenEntity>();

            foreach (var doc in result)
            {
                var entity = new OpenEntity();
                foreach (var item in this.Schema.Query())
                {
                    if (doc.Contains(item.Name))
                    {
                        entity[item.Name] = doc[item.Name];
                    }
                    else
                    {
                        entity[item.Name] = null;
                    }
                }
                list.Add(entity);
            }
            if (translator.Top == 1)
            {
                return list.FirstOrDefault();
            }

            return list;
        }

        public virtual IOpenQuery<T> Query()
        {
            return new OpenQuery<T>(this);
        }
    }
}