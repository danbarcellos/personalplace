using System.Collections.Generic;
using PersonalPlace.Domain.Base.Component;

namespace PersonalPlace.Domain.Base
{
    public abstract class EntityWithStateRecord<TStateType> : EntityWithStaticState<TStateType> 
    {
        protected EntityWithStateRecord()
        {
        }

        protected EntityWithStateRecord(EntityState<TStateType> state, string scopeTag) : base(state, scopeTag)
        {
        }

        private ISet<StateRegister<TStateType>> _states;
        public virtual IEnumerable<StateRegister<TStateType>> States => _states;

        public override void AssignState(EntityState<TStateType> state)
        {
            base.AssignState(state);
            var registroEstado = GetStateRegister(state);
            _states.Add(registroEstado);
        }

        protected virtual StateRegister<TStateType> GetStateRegister(EntityState<TStateType> state)
        {
            return this.GetStateRecord(state);
        }

        protected override void InitializeInternalCollections()
        {
            base.InitializeInternalCollections();
            _states = new HashSet<StateRegister<TStateType>>();
        }
    }
}