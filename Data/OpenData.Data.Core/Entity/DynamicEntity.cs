
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace OpenData.Data.Core
{
    public partial class DynamicEntity : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            return this.dictionary.TryGetValue(name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                var name = binder.Name;
                this.dictionary[name] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dictionary.Keys;
        }
    }
    public partial class DynamicEntity : IDictionary<string, object>
    {
        IDictionary<string, object> dictionary;
        public DynamicEntity()
        {
            this.dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        public void Add(string key, object value)
        {
            this.dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.dictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                return this.dictionary[key];
            }
            set
            {
                this.dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.dictionary.Add(item);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.dictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }
    }
    public partial class DynamicEntity : IEntity
    {
        public virtual string Id
        {
            get
            {
                if (this.ContainsKey("Id"))
                {
                    return (string)this["Id"];
                }
                return string.Empty;
            }
            set
            {
                this["Id"] = value;
            }
        }

        public virtual string EntityName
        {
            get
            {
                if (this.ContainsKey("EntityName"))
                {
                    return this["EntityName"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["EntityName"] = value;
            }
        }
        public DateTime CreatedOn
        {
            get
            {
                if (!this.ContainsKey("CreatedOn"))
                {
                    return DateTime.UtcNow;
                }

                DateTime dt;

                if (DateTime.TryParse(this["CreatedOn"].ToString(), out dt))
                {
                    return dt;
                }
                return DateTime.UtcNow;
            }
            set
            {
                this["CreatedOn"] = value;
            }
        }
        public DateTime UpdatedOn
        {
            get
            {
                if (!this.ContainsKey("UpdatedOn"))
                {
                    return DateTime.UtcNow;
                }

                DateTime dt;

                if (DateTime.TryParse(this["UpdatedOn"].ToString(), out dt))
                {
                    return dt;
                }
                return DateTime.UtcNow;
            }
            set
            {
                this["UpdatedOn"] = value;
            }
        }
        public string CreatedBy
        {
            get
            {
                if (this.ContainsKey("CreatedBy"))
                {
                    return (string)this["CreatedBy"];
                }
                return string.Empty;
            }
            set
            {
                this["CreatedBy"] = value;
            }
        }
        public string UpdatedBy
        {
            get
            {
                if (this.ContainsKey("UpdatedBy"))
                {
                    return (string)this["UpdatedBy"];
                }
                return string.Empty;
            }
            set
            {
                this["UpdatedBy"] = value;
            }
        }
        public int Status
        {
            get
            {
                if (this.ContainsKey("Status"))
                {
                    return (int)this["Status"];
                }
                return 0;
            }
            set
            {
                this["Status"] = value;
            }
        }
        public virtual bool EnableVersion
        {
            get
            {
                if (this.ContainsKey("EnableVersion"))
                {
                    return (bool)this["EnableVersion"];
                }
                return false;
            }
            set
            {
                this["EnableVersion"] = value;
            }
        }
        public virtual bool HasWorkflow
        {
            get
            {
                if (this.ContainsKey("HasWorkflow"))
                {
                    return (bool)this["HasWorkflow"];
                }
                return false;
            }
            set
            {
                this["HasWorkflow"] = value;
            }
        }
    }
}