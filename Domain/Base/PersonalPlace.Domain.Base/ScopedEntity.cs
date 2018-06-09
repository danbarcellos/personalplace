using System;
using System.Linq;
using Pactor.Infra.DAL.ORM;
using PersonalPlace.Domain.Base.Filters;

namespace PersonalPlace.Domain.Base
{
    public abstract class Entidade : Entity
    {
    }

    public abstract class ScopedEntity : Entidade
    {
        private Segregation? _segregationStrategy;

        protected ScopedEntity()
        {
        }

        protected ScopedEntity(string scopeTag)
        {
            ScopeTag = scopeTag ?? DomainContextConstants.EmptyScopeTag;
        }

        public virtual string ScopeTag { get; set; }

        public virtual void SetScopeTag(string scopeTag)
        {
            //var userScopeTag = DomainContext.ScopeTag;

            //if (userScopeTag != DomainContextConstants.DefaultScopeTag && !scopeTag.StartsWith(userScopeTag)) 
            //    throw new DomainArgumentException(string.Format("Scope Tag {0} não pode ser atribuido pelo usuário {1}", scopeTag, DomainContext.UserContext.Login));

            ScopeTag = scopeTag;
        }

        public virtual bool IsAccessible(string scopeTag)
        {
            if (scopeTag == null)
                throw new ArgumentNullException(nameof(scopeTag));

            if (string.IsNullOrWhiteSpace(scopeTag))
                return false;

            if (scopeTag == DomainContextConstants.RootScopeTag)
                return true;

            if (_segregationStrategy == null)
            {
                var entityParameter = GetType()
                    .GetCustomAttributes(true)
                    .SingleOrDefault(ca => ca.GetType() == typeof(EntityParameterAttribute)) as EntityParameterAttribute;

                _segregationStrategy = entityParameter?.Segregation ?? Segregation.Root;
            }
            
            switch (_segregationStrategy)
            {
                case Segregation.Descending:
                    return scopeTag.StartsWith(ScopeTag);
                case Segregation.Ascending:
                    var ascendingScopeTags = scopeTag.ToAscendingScopeTags();
                    return ascendingScopeTags.Contains(ScopeTag);
                case Segregation.Parenthood:
                    var parenthoodAscendingScopeTags = scopeTag.ToParenthoodAscendingScopeTags();
                    return scopeTag.StartsWith(ScopeTag) || parenthoodAscendingScopeTags.Contains(ScopeTag);
            }

            return true;
        }
    }
}