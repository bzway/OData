using OpenData.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenData.Framework.Entity
{
    public class SiteLayout : BaseEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}