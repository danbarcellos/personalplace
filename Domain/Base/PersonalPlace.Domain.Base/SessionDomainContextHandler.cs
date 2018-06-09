using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Persister.Entity;
using PersonalPlace.Domain.Base.Component;

namespace PersonalPlace.Domain.Base
{
    public class SessionDomainContextHandler : IPreInsertEventListener
    {
        private static readonly ConcurrentDictionary<Guid, IDomainContext> SessionContexts = new ConcurrentDictionary<Guid, IDomainContext>();
        private const string StatePropertyName = nameof(EntityWithState<object>.State);
        private const string AssignerUserPropertyName = nameof(EntityState<object>.AssignerUser);

        public static IDisposable SetSessionContext(ISession session, IDomainContext domainContext)
        {
            var sessionId = ((ISessionImplementor)session).SessionId;
            SessionContexts.TryAdd(sessionId, domainContext);
            return new SessionScopeDisposable(sessionId);
        }

        public static IDisposable SetSessionContext(IStatelessSession session, IDomainContext domainContext)
        {
            var sessionId = ((ISessionImplementor)session).SessionId;
            SessionContexts.TryAdd(sessionId, domainContext);
            return new SessionScopeDisposable(sessionId);
        }

        public static IDomainContext GetSessionContext(ISession session)
        {
            var sessionId = ((ISessionImplementor)session).SessionId;
            return SessionContexts.TryGetValue(sessionId, out var domainContext) ? domainContext : null;
        }

        public static IDomainContext GetSessionContext(IStatelessSession session)
        {
            var sessionId = ((ISessionImplementor)session).SessionId;
            return SessionContexts.TryGetValue(sessionId, out var domainContext) ? domainContext : null;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            return Task.Run(() => OnPreInsert(@event), cancellationToken);
        }

        public bool OnPreInsert(PreInsertEvent insertEvent)
        {
            var scopedEntity = insertEvent.Entity as ScopedEntity;

            if (scopedEntity != null && (scopedEntity.ScopeTag == DomainContextConstants.EmptyScopeTag || scopedEntity.ScopeTag == null))
            {
                var domainContext = GetSessionContext(insertEvent.Session);

                if (domainContext == null)
                    return true; // Veto the entity persistence

                Set(insertEvent.Persister, insertEvent.State, nameof(ScopedEntity.ScopeTag), domainContext.ScopeTag);
                scopedEntity.ScopeTag = domainContext.ScopeTag;
            }

            var entityType = insertEvent.Entity.GetType();
            var entitywithStateType = entityType.GetBaseTypes().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(EntityWithState<>));
            if (entitywithStateType != null)
            {
                var entityWithStateStateProperty = entitywithStateType.GetProperty(StatePropertyName, BindingFlags.Instance | BindingFlags.Public);
                var entityState = entityWithStateStateProperty.GetValue(insertEvent.Entity);
                var entityStateAssignerUserProperty = entityState.GetType().GetProperty(AssignerUserPropertyName, BindingFlags.Instance | BindingFlags.Public);
                var assignerUser = (Guid)entityStateAssignerUserProperty.GetValue(entityState);

                if (assignerUser != Guid.Empty)
                    return false;

                var dc = GetSessionContext(insertEvent.Session);
                if (dc == null)
                    return true; // Veto the entity persistence

                entityStateAssignerUserProperty.SetValue(entityState, dc.UserContext.Id);
                Set(insertEvent.Persister, insertEvent.State, StatePropertyName, entityState);
                return false;
            }

            if (entityType.BaseType != null && entityType.BaseType.IsGenericType && entityType.BaseType.GetGenericTypeDefinition() == typeof(StateRegister<>))
            {
                var entityStateRegisterAssignerUserProperty = entityType.GetProperty(nameof(StateRegister<object>.AssignerUser), BindingFlags.Instance | BindingFlags.Public);
                var assignerUser = (Guid)entityStateRegisterAssignerUserProperty.GetValue(insertEvent.Entity);

                if (assignerUser != Guid.Empty)
                    return false;

                var dc = GetSessionContext(insertEvent.Session);
                if (dc == null)
                    return true; // Veto the entity persistence

                entityStateRegisterAssignerUserProperty.SetValue(insertEvent.Entity, dc.UserContext.Id);
                Set(insertEvent.Persister, insertEvent.State, AssignerUserPropertyName, dc.UserContext.Id);
            }

            return false;
        }

        private static void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);

            if (index == -1)
                return;

            state[index] = value;
        }

        private class SessionScopeDisposable : IDisposable
        {
            private readonly Guid _sessionId;

            public SessionScopeDisposable(Guid sessionId)
            {
                _sessionId = sessionId;
            }

            public void Dispose()
            {
                IDomainContext removedDomainContext;
                SessionContexts.TryRemove(_sessionId, out removedDomainContext);
            }
        }
    }
}