using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Framework.Extension
{
    public static class OpenEntityHelper
    {
        public static Schema ToSchema(this IEnumerable<OpenEntity> entities, string schemaName)
        {
            List<Column> list = new List<Column>();
            foreach (var entity in entities)
            {
                list.Add(new Column()
                {
                    Name = (string)entity["Name"],
                    //AllowNull = (bool)entity["AllowNull"],
                    ControlType = (string)entity["ControlType"],
                    //CustomSettings = (string)entity["ControlType"],
                    DefaultValue = (string)entity["ControlType"],
                    Indexable = (bool)entity["ControlType"],
                    IsSystemField = (bool)entity["ControlType"],
                    Label = (string)entity["ControlType"],
                    Length = (int)entity["ControlType"],
                    Modifiable = (bool)entity["ControlType"],
                    Order = (int)entity["ControlType"],
                    SelectionFolder = (string)entity["ControlType"],
                    ShowInGrid = (bool)entity["ControlType"],
                    Summarize = (bool)entity["ControlType"],
                    Tooltip = (string)entity["ControlType"],
                });
            }
            return new Schema(schemaName, list);
        }

        public static IList<Column> ToColumns(this OpenEntity entity)
        {

            var type = entity.GetType();
            var name = type.Name;

            var props = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            List<Column> list = new List<Column>();
            foreach (var item in props)
            {
                var attribute = item.GetCustomAttribute<ColumnAttribute>();
                if (attribute == null)
                {

                    attribute = new ColumnAttribute(item.Name, item.Name);


                }
                Column column = new Column()
                {
                    AllowNull = attribute.AllowNull,
                    ControlType = attribute.ControlType,
                    DefaultValue = attribute.DefaultValue,
                    Indexable = attribute.Indexable,
                    Label = attribute.Label,
                    Length = attribute.Length,
                    Modifiable = attribute.Modifiable,
                    Name = attribute.Name,
                    Order = attribute.Order,
                    ShowInGrid = attribute.ShowInGrid,
                    Tooltip = attribute.Tooltip,
                };
                list.Add(column);
            }
            return list;
        }
    }
}
