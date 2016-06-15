
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Data.Core.Model
{
    public class DataModel : IDataModel
    {
        public Schema DataSchema { get; set; }
        public IEntity Entity { get; set; }
        public List<RelatedEntity> Catetories { get; set; }
        public List<RelatedEntity> Childred { get; set; }
    }
}