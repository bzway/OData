using OpenData.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenData.Framework.Entity
{
    public class PageBlock : BaseEntity
    {
        public string Name { get; set; }
        public string PageId { get; set; }
        public string ViewId { get; set; }
        public int OrderBy { get; set; }
    }
}