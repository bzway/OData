using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace OpenData.Framework
{

    public class Column
    {
        #region System Columns
        public static string[] Sys_Fields = new string[] { 
            "Id",
            "UUID",
            "Repository",
            "FolderName",
            "UserKey",
            "UtcCreationDate",
            "UtcLastModificationDate",
            "Published",           
            "SchemaName",
            "ParentFolder",
            "ParentUUID",
            "UserId",
            "OriginalFolder",
            "OriginalUUID",
            "IsLocalized",
            "Sequence"
        };
        public static Column Id = new Column()
        {
            Name = "Id",
            Label = "Id",
            ControlType = "Hidden"
        };
        public static Column UUID = new Column()
        {
            Name = "UUID",
            Label = "UUID",
            ControlType = "Hidden"
        };
        public static Column UserKey = new Column()
        {
            Name = "UserKey",
            Label = "User Key",
            //DataType = DataType.String,
            ControlType = "TextBox",
            AllowNull = true,
            //ShowInGrid = true,
            DefaultValue = "",
            Order = 100,
            Tooltip = "An user and SEO friendly content key, it is mostly used to customize the page URL"
        };
        public static Column UtcCreationDate = new Column()
        {
            Name = "UtcCreationDate",
            Label = "Creation date",
            AllowNull = true,
            ShowInGrid = true,

            Order = 98,
        };
        public static Column UtcLastModificationDate = new Column()
        {
            Name = "UtcLastModificationDate",
            Label = "UtcLastModificationDate",

            ControlType = "Hidden"
        };
        public static Column Published = new Column()
        {
            Name = "Published",
            Label = "Published",

            ControlType = "Checkbox",
            AllowNull = true,
            ShowInGrid = true,
            DefaultValue = "false",
            Order = 99,
        };
        public static Column ParentFolder = new Column()
        {
            Name = "ParentFolder",
            Label = "ParentFolder",
            ControlType = "Hidden",
            //DataType = DataType.String,
            Order = 99,
        };
        public static Column ParentUUID = new Column()
        {
            Name = "ParentUUID",
            Label = "ParentUUID",
            ControlType = "Hidden",
            //DataType = DataType.String,
            Order = 99,
        };
        public static Column UserId = new Column()
        {
            Name = "UserId",
            Label = "UserId",
            ControlType = "Hidden"
        };

        public static Column OriginalFolder = new Column()
        {
            Name = "OriginalFolder",
            Label = "OriginalFolder",
            ControlType = "Hidden",
            //DataType = DataType.String,
            Order = 99,
        };
        public static Column OriginalUUID = new Column
        {
            Name = "OriginalUUID",
            Label = "OriginalUUID",
            ControlType = "Hidden"
        };
        public static Column IsLocalized = new Column
        {
            Name = "IsLocalized",
            Label = "IsLocalized",
            //DataType = DataType.Bool,
            ControlType = "Hidden"
        };

        public static Column Sequence = new Column()
        {
            Name = "Sequence",
            Label = "Sequence",
            ControlType = "Hidden",
            //DataType = DataType.Int,
            Order = 99,
        };
        #endregion

        #region Persistence field
        /// <summary>
        /// Gets or sets the name.
        /// Unable to update.
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public string Label { get; set; }
        [DataMember(Order = 6)]
        public string ControlType { get; set; }
        private bool allowNull = true;
        [DataMember(Order = 7)]
        public bool AllowNull { get { return allowNull; } set { allowNull = value; } }
        [DataMember(Order = 9)]
        public int Length { get; set; }
        [DataMember(Order = 11)]
        public int Order { get; set; }
        //[DataMember(Order = 13)]
        //public bool Queryable { get; set; }
        private bool modifiable = true;
        [DataMember(Order = 15)]
        public bool Modifiable { get { return modifiable; } set { modifiable = value; } }
        private bool indexable = true;
        [DataMember(Order = 17)]
        public bool Indexable { get { return indexable; } set { indexable = value; } }
        [DataMember(Order = 19)]
        public bool ShowInGrid { get; set; }
        [DataMember(Order = 21)]
        public string Tooltip { get; set; }


        [DataMember(Order = 24)]
        public string SelectionFolder { get; set; }


        [DataMember(Order = 27)]
        public string DefaultValue { get; set; }
        [DataMember(Order = 29)]
        public bool Summarize { get; set; }
        private Dictionary<string, string> customSettings;
        [DataMember(Order = 31)]
        public Dictionary<string, string> CustomSettings
        {
            get { return customSettings ?? (customSettings = new Dictionary<string, string>()); }
            set
            {
                if (value != null)
                {
                    customSettings = new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    customSettings = value;
                }

            }
        }
        #endregion

        #region override object
        public static bool operator ==(Column obj1, Column obj2)
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
        public static bool operator !=(Column obj1, Column obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Column))
            {
                return false;
            }
            if (obj == null)
            {
                return false;
            }
            if (string.Compare(this.Name, ((Column)obj).Name, true) == 0)
            {
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Name.ToLower().GetHashCode();
        }
        #endregion

        #region Clone
        public Column DeepClone()
        {
            var column = (Column)this.MemberwiseClone();
            return column;
        }
        #endregion

        #region IsSystemField
        public bool IsSystemField
        {
            get
            {
                return Sys_Fields.Where(m => m.Equals(this.Name, StringComparison.OrdinalIgnoreCase)).Count() > 0;
            }
            set { }
        }
        #endregion
    }

    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name, string label = "", string controlType = "input", bool allowNull = true,
        int length = 0, int order = 0, bool modifiable = true, bool indexable = false,
            bool showInGrid = true, string toolTip = "", string defaultValue = "", string RegExp = "")
        {

        }
        public string Name { get; set; }
        public string Label { get; set; }
        public string ControlType { get; set; }
        public bool AllowNull { get; set; }
        public int Length { get; set; }
        public int Order { get; set; }
        public bool Modifiable { get; set; }
        public bool Indexable { get; set; }
        public bool ShowInGrid { get; set; }
        public string Tooltip { get; set; }
        public string DefaultValue { get; set; }
        public string RegExp { get; set; }
    }
}