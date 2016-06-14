using OpenData.Data.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace OpenData.Data.Core
{
    public class RelatedEntity : BaseEntity, IRelatedEntity
    {
        public string DisplayName { get; set; }

        public RelationType OneToOne { get; set; }

        public List<IEntity> Relates { get; set; }
    }

    public interface IRelatedEntity : IEntity
    {
        RelationType OneToOne { get; set; }

        string DisplayName { get; set; }

        List<IEntity> Relates { get; set; }
    }

    public enum RelationType
    {
        OneToZero,
        OneToOne,
        OneToMany,
    }
}