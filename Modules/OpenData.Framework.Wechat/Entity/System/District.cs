using OpenData.Data;
using System;

namespace OpenData.Framework.Entity
{

    public class District : BaseEntity
    {
        public string CityID { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}