using System;

namespace Pactor.Infra.DAL.ORM.Queries
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryConstraintAttribute : Attribute
    {
        public bool Scoped { get; set; } = true;
        public bool StatelessEnable { get; set; } = true;
    }
}