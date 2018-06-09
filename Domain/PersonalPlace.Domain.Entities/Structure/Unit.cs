using System;
using System.Threading;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Base.Component;
using PersonalPlace.Domain.Common.ValueObjects;

namespace PersonalPlace.Domain.Entities.Structure
{
    [EntityParameter(Segregation.Descending, isAggregateRoot: true)]
    public class Unit : EntityWithStaticState<UnitStateType>
    {
        protected Unit()
        {
        }

        public Unit(string description, Unit upperUnit = null, UnitStateType unitStateType = UnitStateType.Active, int hierarchicalSequence = 1, string scopeTag = null)
            : this(description, upperUnit, new EntityState<UnitStateType>(unitStateType), hierarchicalSequence, scopeTag)
        {
        }

        public Unit(string description, Unit upperUnit, EntityState<UnitStateType> state, int hierarchicalSequence = 1, string scopeTag = null)
            : base(state ?? new EntityState<UnitStateType>(UnitStateType.Active), scopeTag ?? (upperUnit == null ? DomainContextConstants.RootScopeTag : upperUnit.GetNewHierarchicalSubordination()))
        {
            InitializeInternalCollections();
            UpperUnit = upperUnit;
            Description = description;
            HierarchicalSequence = hierarchicalSequence;
            Culture = upperUnit == null ? Thread.CurrentThread.CurrentCulture.Name : upperUnit.Culture ?? Thread.CurrentThread.CurrentCulture.Name;
        }

        public virtual string ExternalId { get; set; }

        public virtual Guid? ExternalSourceId { get; set; }

        public virtual int? DataLineNumber { get; set; }

        public virtual Unit UpperUnit { get; set; }

        public virtual string Description { get; set; }

        public virtual string PhotoFilePrefix { get; set; }

        public virtual int HierarchicalSequence { get; protected set; }

        public virtual string Culture { get; set; }

        public virtual string GetNewHierarchicalSubordination()
        {
            return ScopeTag + HierarchicalSequence++.ToString("D") + ".";
        }
    }
}
