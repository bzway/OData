using System.Collections.Generic;

namespace OpenData.Data.Core.Model
{
    public interface IDataModel
    {
        List<RelatedEntity> Catetories { get; set; }
        List<RelatedEntity> Childred { get; set; }
        Schema DataSchema { get; set; }
        IEntity Entity { get; set; }
    }
}