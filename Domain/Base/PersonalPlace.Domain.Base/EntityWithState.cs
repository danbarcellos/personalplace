using System;
using PersonalPlace.Domain.Base.Component;

namespace PersonalPlace.Domain.Base
{
    public abstract class EntityWithState<TTipoEstado> : ScopedEntity
    {
        protected Action<EntityState<TTipoEstado>> StateValidator = estado => { };

        protected EntityWithState()
        {
        }

        protected EntityWithState(string scopeTag) : base(scopeTag)
        {
        }

        public virtual EntityState<TTipoEstado> State { get; protected set; }
    }
}