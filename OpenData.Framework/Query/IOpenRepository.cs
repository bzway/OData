using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Framework
{
    public interface IOpenRepository<T> where T : OpenEntity
    {
        ISchemaRepository Schema { get; set; }
        void Insert(T newData);

        void Update(T newData, T oldData);
        void Delete(T oldData);
        object Execute(IOpenQuery<T> query);

        IOpenQuery<T> Query();
    }

    public interface ISchemaRepository
    {
        string Name { get; set; }
        void Insert(Column column);
        IEnumerable<Column> Query();
        void Delete(Column column);
    }
}