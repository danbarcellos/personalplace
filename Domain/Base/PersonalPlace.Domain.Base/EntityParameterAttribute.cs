using System;

namespace PersonalPlace.Domain.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EntityParameterAttribute : Attribute
    {
        public EntityParameterAttribute(Segregation segregation = Segregation.Descending, bool isAggregateRoot = false)
        {
            Segregation = segregation;
            IsAggregateRoot = isAggregateRoot;
        }

        public Segregation Segregation { get; set; }

        public bool IsAggregateRoot { get; set; }
    }

    public enum Segregation
    {
        Root = 1,
        Descending = 2,
        Ascending = 3,
        Parenthood = 4
    }
}