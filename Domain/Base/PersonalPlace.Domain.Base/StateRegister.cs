using System;
using PersonalPlace.Domain.Base.Component;

namespace PersonalPlace.Domain.Base
{
    public abstract class StateRegister<TTipoEstado> : ScopedEntity
    {
        protected StateRegister()
        {
        }

        protected StateRegister(EntityState<TTipoEstado> state, string scopeTag = null) : base(scopeTag)
        {
            StateType = state.StateType;
            StateDateTime = state.StateDateTime;
            AssignerUser = state.AssignerUser;
            StateNote = state.StateNote;
        }

        public virtual TTipoEstado StateType { get; protected set; }

        public virtual DateTimeOffset StateDateTime { get; protected set; }

        public virtual Guid AssignerUser { get; protected set; }

        public virtual string StateNote { get; protected set; }

        protected internal virtual void RegistrarEstado(EntityState<TTipoEstado> state)
        {
        }
    }
}