using OpenData.Data;
using System;

namespace OpenData.Framework.Entity
{

    public class City : BaseEntity
    {
        public string ProinceID { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}