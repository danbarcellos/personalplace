using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pactor.Infra.DAL.ORM;
using PersonalPlace.Domain.Base.Component;

namespace PersonalPlace.Domain.Base
{
    public class DomainContextInjectionInterceptor : IPreInsertInterceptor
    {
        private const string StatePropertyName = nameof(EntityWithState<object>.State);
        private const string StatesPropertyName = nameof(EntityWithStateRecord<object>.States);
        private const string StateRegistryIdPropertyName = nameof(StateRegister<object>.Id);
        private const string StateRegistryAssignerUserPropertyName = nameof(StateRegister<object>.AssignerUser);
        private const string AssignerUserPropertyName = nameof(EntityState<object>.AssignerUser);

        private readonly IDomainContext _domainContext;

        public DomainContextInjectionInterceptor(IDomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        public string Description => "Domain context injection interceptor";

        public InterceptorResult OnPreInsert<TEntity>(TEntity entity) where TEntity : Entity
        {
            var scopedEntity = entity as ScopedEntity;

            if (scopedEntity == null)
                return new InterceptorResult();

            if (scopedEntity.ScopeTag == DomainContextConstants.EmptyScopeTag || string.IsNullOrWhiteSpace(scopedEntity.ScopeTag))
                scopedEntity.ScopeTag = _domainContext.ScopeTag;

            var entityType = scopedEntity.GetType();
            var stateRegisters = GetStateRegisters(scopedEntity);

            if (stateRegisters != null && stateRegisters.Any())
            {
                var stateRegistryIdProperty = stateRegisters.First().GetType().GetProperty(StateRegistryIdPropertyName, BindingFlags.Instance | BindingFlags.Public);
                var stateRegisterAssignerUserProperty = stateRegisters.First().GetType().GetProperty(StateRegistryAssignerUserPropertyName, BindingFlags.Instance | BindingFlags.Public);

                foreach (var stateRecord in stateRegisters)
                {
                    if (stateRecord.ScopeTag == DomainContextConstants.EmptyScopeTag || string.IsNullOrWhiteSpace(stateRecord.ScopeTag))
                        stateRecord.ScopeTag = scopedEntity.ScopeTag;

                    // ReSharper disable once PossibleNullReferenceException
                    var stateRecordId = (Guid)stateRegistryIdProperty.GetValue(stateRecord);

                    if (stateRecordId != Entity.UnsavedId)
                        continue;

                    // ReSharper disable once PossibleNullReferenceException
                    var registerAssignerUser = (Guid)stateRegisterAssignerUserProperty.GetValue(stateRecord);

                    if (registerAssignerUser == Guid.Empty)
                        stateRegisterAssignerUserProperty.SetValue(stateRecord, _domainContext.UserContext.Id);
                }
            }
            else
            {
                if (!IsSubclassOfRawGeneric(entityType, typeof(EntityWithState<>)))
                    return new InterceptorResult();
            }

            var entityWithStateStateProperty = entityType.GetProperty(StatePropertyName, BindingFlags.Instance | BindingFlags.Public);
            // ReSharper disable once PossibleNullReferenceException
            var entityState = entityWithStateStateProperty.GetValue(scopedEntity);
            var entityStateAssignerUserProperty = entityState.GetType().GetProperty(AssignerUserPropertyName, BindingFlags.Instance | BindingFlags.Public);
            // ReSharper disable once PossibleNullReferenceException
            var assignerUser = (Guid)entityStateAssignerUserProperty.GetValue(entityState);

            if (assignerUser != Guid.Empty)
                return new InterceptorResult();

            entityStateAssignerUserProperty.SetValue(entityState, _domainContext.UserContext.Id);

            return new InterceptorResult();
        }

        private static ScopedEntity[] GetStateRegisters(Entity entity)
        {
            var entityType = entity.GetType();
            if (!IsSubclassOfRawGeneric(entityType, typeof(EntityWithStateRecord<>)))
                return null;

            var statesProperty = entityType.GetProperty(StatesPropertyName, BindingFlags.Instance | BindingFlags.Public);
            var entityStates = statesProperty?.GetValue(entity) as IEnumerable<object>;
            return entityStates?.Cast<ScopedEntity>().ToArray();
        }

        private static bool IsSubclassOfRawGeneric(Type type, Type openGenericType)
        {
            var toCheck = type;
            while (toCheck != null && toCheck != typeof(object))
            {
                var genericDefinition = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

                if (openGenericType == genericDefinition)
                    return true;

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}