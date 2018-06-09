using System;
using PersonalPlace.Domain.Base.Component;
using PersonalPlace.Domain.Common.Exception;

namespace PersonalPlace.Domain.Base
{
    public abstract class EntityWithStaticState<TStateType> : EntityWithState<TStateType>
    {
        protected EntityWithStaticState()
        {
            InitializeInternalCollections();
        }

        protected EntityWithStaticState(EntityState<TStateType> state, string scopeTag) : base(scopeTag)
        {
            InitializeInternalCollections();
            AssignState(state);
        }

        public virtual void AssignState(TStateType state, string stateNote = null)
        {
            AssignState(state, DateTime.Now, stateNote);
        }

        public virtual void AssignState(TStateType state, DateTime dateTime, string stateNote = null)
        {
            var entityState = new EntityState<TStateType>(state, dateTime, stateNote);
            AssignState(entityState);
        }

        public virtual void AssignState(EntityState<TStateType> state)
        {
            if (state == null)
                throw new ArgumentNullException("state", "Error: Attempt to assign a null state to the entity.");

            var currentState = State;

            if (currentState != null)
            {
                if (state.StateDateTime < currentState.StateDateTime)
                    throw new DomainStateEntityException("Error: Attempt to assign a state dated prior to the current state of entity.");
            }

            StateValidator(state);
            State = state;
        }
        
        protected virtual void InitializeInternalCollections()
        {
        }
    }
}