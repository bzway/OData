﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Data.Model
{ 
    public class DataModel
    {
        public Schema DataSchema { get; set; }
        public DynamicEntity Entity { get; set; }

        public List<RelatedEntity> Catetories { get; set; }

        public List<RelatedEntity> Childred { get; set; }
    }
}