using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;


namespace OpenData.Framework
{
    public partial class Schema
    {
        public Schema(string name, IEnumerable<Column> columns)
        {
            this.Name = name;

            if (columns != null)
            {
                this.columns.AddRange(columns);
            }
        }
        [DataMember(Order = 1)]
        public string Name { get; set; }



        private List<Column> columns = new List<Column>();
        [DataMember(Order = 7)]
        public List<Column> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        public bool AddColumn(Column column)
        {
            if (this.AllColumns.Contains(column))
            {
                return false;
            }

            this.columns.Add(column);
            return true;
        }

        public int RemoveColumn(Column column)
        {
            var index = this.columns.IndexOf(column);
            this.columns.Remove(column);
            return index;
        }
        public int UpdateColumn(Column oldColumn, Column newColumn)
        {
            var index = RemoveColumn(oldColumn);
            this.columns.Insert(0, newColumn);
            return index;
        }

    }

    public partial class Schema
    {

        #region IPersistable Members
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }


        #endregion

        #region override object
        public static bool operator ==(Schema obj1, Schema obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Schema obj1, Schema obj2)
        {
            return !(obj1 == obj2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return this.Name;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        #endregion

        public Column this[string columnName]
        {
            get
            {
                return this.AllColumns.Where(it => string.Compare(it.Name, columnName, true) == 0).FirstOrDefault();
            }
        }

        public Column GetSummarizeColumn()
        {
            var summarizeField = this.columns.OrderBy(it => it.Order).Where(it => it.Summarize == true).FirstOrDefault();
            if (summarizeField == null)
            {
                summarizeField = this.columns.OrderBy(it => it.Order).FirstOrDefault();
            }
            if (summarizeField == null)
            {
                summarizeField = Column.UserKey;
            }
            return summarizeField;
        }

        public Schema DeepClone()
        {
            var schema = (Schema)this.MemberwiseClone();
            if (this.Columns != null)
            {
                schema.columns = new List<Column>();

                foreach (var item in this.Columns)
                {
                    schema.AddColumn(item.DeepClone());
                }
            }
            return schema;
        }

        IEnumerable<Column> sysColumns = new[] {
                    Column.Id,
                    Column.UUID,
                    Column.UserKey,
                    Column.UtcCreationDate,
                    Column.UtcLastModificationDate,
                    Column.Published,
                    Column.ParentFolder,
                    Column.ParentUUID,
                    Column.UserId,
                    Column.OriginalFolder,
                    Column.OriginalUUID,
                    Column.IsLocalized,
                    Column.Sequence,};
        public List<Column> SystemColumns
        {
            get
            {
                return sysColumns.ToList();
            }
        }
        public List<Column> AllColumns
        {
            get
            {

                if (this.Columns == null)
                {
                    return this.SystemColumns;
                }
                return this.Columns.OrderBy(o => o.Order).Concat(sysColumns).Distinct().ToList();
            }
        }
    }
}