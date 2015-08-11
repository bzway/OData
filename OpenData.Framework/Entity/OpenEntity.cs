using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace OpenData.Framework
{
    public partial class OpenEntity : DynamicObject, IDictionary<string, object>
    {
        IDictionary<string, object> dictionary;
        public dynamic Entity { get { return this; } }
        public OpenEntity()
        {
            dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            return dictionary.TryGetValue(name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                var name = binder.Name;
                dictionary[name] = value;
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

    public partial class OpenEntity
    {
        [Key]
        public int ID
        {
            get
            {
                if (this.ContainsKey("ID"))
                {
                    return int.Parse(this["ID"].ToString());
                }
                return 0;
            }
            set
            {
                this["ID"] = value;
            }
        }
        public string UUID
        {
            get
            {
                if (this.ContainsKey("UUID"))
                {
                    return (string)this["UUID"];
                }
                return string.Empty;
            }
            set
            {
                this["UUID"] = value;
            }
        }
        public DateTime CreatedOn
        {
            get
            {
                if (!this.ContainsKey("CreatedOn"))
                {
                    return DateTime.Now;
                }

                DateTime dt;

                if (DateTime.TryParse(this["CreatedOn"].ToString(), out dt))
                {
                    return dt;
                }
                return DateTime.Now;
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
                    return DateTime.Now;
                }

                DateTime dt;

                if (DateTime.TryParse(this["UpdatedOn"].ToString(), out dt))
                {
                    return dt;
                }
                return DateTime.Now;
            }
            set
            {
                this["UpdatedOn"] = value;
            }
        }
    }
}
