using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenData.Common;
using OpenData.Data.Core.Query;
using OpenData.Data.Core.Query.OpenExpressions;
using OpenData.Data.Core;

namespace OpenData.Data.Mongo
{
    public class BaseEntityMongoRepository<T> : IRepository<T> where T : new()
    {
        #region ctor
        readonly string connectionString;
        readonly string datebaseName;
        readonly string collectionName;
        Schema schema { get; set; }
        public BaseEntityMongoRepository(string connectionString, string databaseName, Schema schema)
        {
            this.schema = schema;
            this.connectionString = connectionString;
            this.datebaseName = databaseName;
            this.collectionName = schema.Name;
        }

        #endregion

        #region Insert
        public virtual void Insert(T newData)
        {
            BsonDocument insert;
            if (newData is DynamicEntity)
            {
                var t = (DynamicEntity)Convert.ChangeType(newData, typeof(DynamicEntity));
                insert = GetInsertDocument(t);
            }
            else
            {
                insert = new BsonDocument();
                var uuid = newData.TryGetValue("Id");
                if (uuid == null || uuid.ToString() == string.Empty)
                {
                    insert.Add("Id", Guid.NewGuid().ToString("N"));
                }
                else
                {
                    insert.Add("Id", uuid.ToString());
                }
                insert.Add("CreatedOn", BsonValue.Create(newData.TryGetValue("CreatedOn")));
                insert.Add("UpdatedOn", BsonValue.Create(newData.TryGetValue("UpdatedOn")));
                insert.Add("CreatedBy", BsonValue.Create(newData.TryGetValue("CreatedBy")));
                insert.Add("UpdatedBy", BsonValue.Create(newData.TryGetValue("UpdatedBy")));
                insert.Add("Status", BsonValue.Create(newData.TryGetValue("Status")));
                foreach (var item in this.schema.AllColumns.Where(m => !m.IsSystemField))
                {
                    if (newData.TryGetValue(item.Name) != null)
                    {
                        insert.Add(item.Name, BsonValue.Create(newData.TryGetValue(item.Name)));
                    }
                    else
                    {
                        insert.Add(item.Name, BsonValue.Create(item.DefaultValue));
                    }
                }
            }

            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var result = collection.InsertOneAsync(insert);
        }
        private BsonDocument GetInsertDocument(DynamicEntity newData)
        {
            var insert = new BsonDocument();
            if (string.IsNullOrEmpty(newData.Id))
            {
                insert.Add("Id", Guid.NewGuid().ToString("N"));
            }
            else
            {
                insert.Add("Id", newData.Id);
            }
            insert.Add("CreatedOn", newData.CreatedOn);
            insert.Add("UpdatedOn", newData.UpdatedOn);
            insert.Add("CreatedBy", newData.CreatedBy);
            insert.Add("UpdatedBy", newData.UpdatedBy);
            insert.Add("Status", newData.Status);
            foreach (var item in this.schema.AllColumns.Where(m => !m.IsSystemField))
            {
                if (newData.ContainsKey(item.Name))
                {
                    insert.Add(item.Name, BsonValue.Create(newData[item.Name]));
                }
                else
                {
                    insert.Add(item.Name, BsonValue.Create(item.DefaultValue));
                }
            }
            return insert;
        }
        #endregion

        #region Delete
        public virtual void Delete(T oldData)
        {
            string uuid;
            if (oldData is DynamicEntity)
            {
                var t = (DynamicEntity)Convert.ChangeType(oldData, typeof(DynamicEntity));
                uuid = t.Id;
            }
            else
            {
                var t = oldData.TryGetValue("uuid");
                if (t == null)
                {
                    return;
                }
                uuid = t.ToString();
            }
            this.Delete(uuid);
        }
        public void Delete(string uuid)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("Id", uuid);
            var result = collection.DeleteOneAsync(filter);
        }
        #endregion

        #region Update
        public virtual void Update(T newData, string uuid = "")
        {
            if (string.IsNullOrEmpty(uuid))
            {
                if (newData.TryGetValue("Id") == null)
                {
                    return;
                }
                uuid = newData.TryGetValue("Id").ToString();
            }
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("Id", uuid);
            var update = Builders<BsonDocument>.Update.CurrentDate("UpdatedOn");
            foreach (var item in this.schema.AllColumns.Where(m => !m.IsSystemField))
            {
                if (newData.TryGetValue(item.Name) != null)
                {
                    update = Builders<BsonDocument>.Update.Set(item.Name, BsonValue.Create(newData.TryGetValue(item.Name)));
                    collection.UpdateOneAsync(filter, update);
                }
            }
            collection.UpdateOneAsync(filter, update);
        }
        public IUpdate<T> Update(IWhereExpression where)
        {
            return new OpenUpdate<T>(this, where);
        }
        public bool Execute(IUpdate<T> update)
        {
            return true;
        }
        #endregion
     
        #region Query
        public IWhere<T> Filter()
        {
            return new OpenWhere<T>();
        }
        public IOpenQuery<T> Query(params string[] fields)
        {
            return new OpenQuery<T>(this, fields);
        }
        public virtual object Execute(IOpenQuery<T> query)
        {
            switch (query.CallExpression.CallType)
            {
                case CallType.Count:
                    return Count(query);
                case CallType.First:
                    return First(query);
                case CallType.Last:
                    return Last(query);
                case CallType.ToList:
                    return ToList(query);
                case CallType.PageList:
                    return PageList(query);
                default:
                    return null;
            }
        }
        private List<T> ToList(IOpenQuery<T> query)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = GetFilter(query);
            if (filter == null)
            {
                filter = new BsonDocument();
            }
            var result = collection.Find(filter);

            var sort = GetSort(query);
            if (sort != null)
            {
                result = result.Sort(sort);
            }

            if (query.TakeExpression.Skip > 0)
            {
                result = result.Limit(query.TakeExpression.Take);
            }
            if (query.TakeExpression.Skip > 0)
            {
                result = result.Skip(query.TakeExpression.Skip);
            }
            List<T> list = new List<T>();
            foreach (var doc in result.ToListAsync().GetAwaiter().GetResult())
            {
                var entity = new T();
                foreach (var item in this.schema.AllColumns)
                {
                    if (doc.Contains(item.Name))
                    {
                        entity.TrySetValue(item.Name, BsonTypeMapper.MapToDotNetValue(doc[item.Name]));
                    }
                    else
                    {
                        entity.TrySetValue(item.Name, null);
                    }
                }
                list.Add(entity);
            }
            return list;
        }
        private PagedList<T> PageList(IOpenQuery<T> query)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);

            var filter = GetFilter(query);
            if (filter == null)
            {
                filter = new BsonDocument();
            }
            var result = collection.Find(filter);

            var sort = GetSort(query);
            if (sort != null)
            {
                result = result.Sort(sort);
            }

            if (query.TakeExpression.Skip > 0)
            {
                result = result.Limit(query.TakeExpression.Take);
            }
            if (query.TakeExpression.Skip > 0)
            {
                result = result.Skip(query.TakeExpression.Skip);
            }
            List<T> list = new List<T>();
            foreach (var doc in result.ToListAsync().GetAwaiter().GetResult())
            {
                var entity = new T();
                foreach (var item in this.schema.AllColumns)
                {
                    if (doc.Contains(item.Name))
                    {
                        entity.TrySetValue(item.Name, BsonTypeMapper.MapToDotNetValue(doc[item.Name]));
                    }
                    else
                    {
                        entity.TrySetValue(item.Name, null);
                    }
                }
                list.Add(entity);
            }

            return new PagedList<T>(list, query.TakeExpression.Skip / query.TakeExpression.Take + 1, query.TakeExpression.Take, Count(query));
        }
        private T Last(IOpenQuery<T> query)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);

            var filter = GetFilter(query);
            if (filter == null)
            {
                filter = new BsonDocument();
            }
            var result = collection.Find(filter);

            var sort = GetSort(query, true);
            if (sort != null)
            {
                result = result.Sort(sort);
            }
            result = result.Limit(1);

            foreach (var doc in result.ToListAsync().GetAwaiter().GetResult())
            {
                var entity = new T();
                foreach (var item in this.schema.AllColumns)
                {
                    if (doc.Contains(item.Name))
                    {
                        entity.TrySetValue(item.Name, BsonTypeMapper.MapToDotNetValue(doc[item.Name]));
                    }
                    else
                    {
                        entity.TrySetValue(item.Name, null);
                    }
                }
                return (entity);
            }
            return default(T);
        }
        private T First(IOpenQuery<T> query)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = GetFilter(query);
            if (filter == null)
            {
                filter = new BsonDocument();
            }
            var result = collection.Find(filter);

            var sort = GetSort(query);
            if (sort != null)
            {
                result = result.Sort(sort);
            }
            result = result.Limit(1);

            foreach (var doc in result.ToListAsync().GetAwaiter().GetResult())
            {
                var entity = new T();
                foreach (var item in this.schema.AllColumns)
                {
                    if (doc.Contains(item.Name))
                    {
                        entity.TrySetValue(item.Name, BsonTypeMapper.MapToDotNetValue(doc[item.Name]));
                    }
                    else
                    {
                        entity.TrySetValue(item.Name, null);
                    }
                }
                return (entity);
            }
            return default(T);
        }
        private int Count(IOpenQuery<T> query)
        {
            var collection = new MongoClient(this.connectionString).GetDatabase(this.datebaseName).GetCollection<BsonDocument>(collectionName);
            var filter = GetFilter(query);
            if (filter == null)
            {
                filter = new BsonDocument();
            }
            var result = collection.Find(filter).CountAsync().GetAwaiter().GetResult();
            return (int)result;
        }
        private FilterDefinition<BsonDocument> GetFilter(IOpenQuery<T> query)
        {
            FilterDefinition<BsonDocument> filter = null;

            if (query.WhereExpression != null)
            {
                if (query.WhereExpression is WhereExpression)
                {
                    filter = GetAndFilter((WhereExpression)query.WhereExpression);
                }
                else
                {
                    filter = GetOrFilter((OrAlsoExpression)query.WhereExpression);
                }
            }
            return filter;
        }
        private FilterDefinition<BsonDocument> GetAndFilter(WhereExpression where)
        {
            FilterDefinition<BsonDocument> filter = null;
            switch (where.CompareType)
            {
                case CompareType.Equal:
                    filter = Builders<BsonDocument>.Filter.Eq(where.FieldName, where.Value);
                    break;
                case CompareType.NotEqual:
                    filter = Builders<BsonDocument>.Filter.Ne(where.FieldName, where.Value);
                    break;
                case CompareType.Like:
                    if (where.Value != null)
                    {
                        filter = Builders<BsonDocument>.Filter.Regex(where.FieldName, new BsonRegularExpression(where.Value.ToString(), "-i"));
                    }
                    else
                    {
                        filter = new BsonDocument();
                    }
                    break;
                case CompareType.GreaterThan:
                    filter = Builders<BsonDocument>.Filter.Gt(where.FieldName, where.Value);
                    break;

                case CompareType.GreaterThanOrEqual:
                    filter = Builders<BsonDocument>.Filter.Gte(where.FieldName, where.Value);
                    break;

                case CompareType.LessThan:
                    filter = Builders<BsonDocument>.Filter.Lt(where.FieldName, where.Value);
                    break;

                case CompareType.LessThanOrEqual:
                    filter = Builders<BsonDocument>.Filter.Lte(where.FieldName, where.Value);
                    break;
                case CompareType.Startwith:
                    if (where.Value != null)
                    {
                        filter = Builders<BsonDocument>.Filter.Regex(where.FieldName, new BsonRegularExpression("^" + where.Value.ToString(), "-i"));
                    }
                    else
                    {
                        filter = new BsonDocument();
                    }

                    break;
                case CompareType.EndWith:
                    if (where.Value != null)
                    {
                        filter = Builders<BsonDocument>.Filter.Regex(where.FieldName, new BsonRegularExpression(where.Value.ToString() + "$", "-i"));
                    }
                    else
                    {
                        filter = new BsonDocument();
                    }

                    break;
                case CompareType.Contains:

                    break;
                case CompareType.NoLike:
                    if (where.Value != null)
                    {
                        filter = Builders<BsonDocument>.Filter.Regex(where.FieldName, new BsonRegularExpression("!" + where.Value.ToString()));
                    }
                    else
                    {
                        filter = new BsonDocument();
                    }

                    break;

                default:
                    filter = Builders<BsonDocument>.Filter.Eq(where.FieldName, where.Value);
                    break;
            }
            if (where.Expression == null)
            {
                return filter;
            }
            if (where.Expression is OrAlsoExpression)
            {
                return filter & GetOrFilter((OrAlsoExpression)where.Expression);
            }
            else
            {
                return filter & GetAndFilter((WhereExpression)where.Expression);
            }
        }
        private FilterDefinition<BsonDocument> GetOrFilter(OrAlsoExpression or)
        {
            FilterDefinition<BsonDocument> filterLeft;
            if (or.Left is WhereExpression)
            {
                filterLeft = GetAndFilter((WhereExpression)or.Left);
            }
            else
            {
                filterLeft = GetOrFilter((OrAlsoExpression)or.Left);
            }
            FilterDefinition<BsonDocument> filterRight;
            if (or.Right is WhereExpression)
            {
                filterRight = GetAndFilter((WhereExpression)or.Right);
            }
            else
            {
                filterRight = GetOrFilter((OrAlsoExpression)or.Right);
            }
            return Builders<BsonDocument>.Filter.Or(filterLeft, filterRight);
        }
        private SortDefinition<BsonDocument> GetSort(IOpenQuery<T> query, bool reverse = false)
        {
            SortDefinition<BsonDocument> sort = null;
            var item = query.OrderExpression;
            while (item != null)
            {
                if (item.Descending & !reverse)
                {
                    if (sort == null)
                    {
                        sort = Builders<BsonDocument>.Sort.Descending(item.FieldName);
                    }
                    else
                    {
                        sort.Descending(item.FieldName);
                    }
                }
                else
                {
                    if (sort == null)
                    {
                        sort = Builders<BsonDocument>.Sort.Ascending(item.FieldName);
                    }
                    else
                    {
                        sort.Ascending(item.FieldName);
                    }
                }
                item = item.Expression;
            }

            return sort;
        }
        #endregion
     
    }
    
    public class EntityMongoRepository<T> : BaseEntityMongoRepository<T> where T : BaseEntity, new()
    {
        public EntityMongoRepository(string connectionString, string databaseName, Schema schema)
            : base(connectionString, databaseName, schema)
        {

        }
    }
    public class DynamicEntityMongoRepository<T> : BaseEntityMongoRepository<T> where T : DynamicEntity, new()
    {
        public DynamicEntityMongoRepository(string connectionString, string databaseName, Schema schema)
            : base(connectionString, databaseName, schema)
        {
        }
    }
}