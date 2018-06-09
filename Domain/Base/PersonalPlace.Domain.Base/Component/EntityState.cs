using System;

namespace PersonalPlace.Domain.Base.Component
{
    public class EntityState<T>
    {
        protected EntityState()
        {
        }

        public EntityState(T stateType, string stateNote = null)
            : this(stateType, DateTimeOffset.Now, stateNote)
        {
        }

        public EntityState(T stateType, DateTimeOffset stateDateTime, string stateNote = null)
            : this(stateType, stateDateTime, Guid.Empty, stateNote)
        {
        }

        public EntityState(T stateType, Guid usuarioAtribuidor, string stateNote = null)
            : this(stateType, DateTimeOffset.Now, usuarioAtribuidor, stateNote)
        {
        }

        public EntityState(T stateType, DateTimeOffset stateDateTime, Guid assignerUser, string stateNote = null)
        {
            StateType = stateType;
            StateDateTime = stateDateTime;
            AssignerUser = assignerUser;
            StateNote = stateNote;
        }

        public virtual T StateType { get; protected set; }

        public virtual DateTimeOffset StateDateTime { get; protected set; }

        public virtual Guid AssignerUser { get; protected set; }

        public virtual string StateNote { get; protected set; }

        public virtual string GetStateDescription()
        {
            //todo: Implementar descrição dos Tipos de Estado vindo de um arquivo de resource.
            return null;
        }
    }
}