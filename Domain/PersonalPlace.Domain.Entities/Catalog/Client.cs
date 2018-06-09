using System.Collections.Generic;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Common.Exception;
using PersonalPlace.Domain.Entities.Security;

namespace PersonalPlace.Domain.Entities.Catalog
{
    [EntityParameter(Segregation.Parenthood, isAggregateRoot: true)]
    public class Client : ScopedEntity
    {
        protected Client()
        {
            InitializeInternalCollections();
        }

        public Client(User user,
                      string scopeTag = null)
            : base(scopeTag ?? user.ScopeTag)
        {
            User = user;
            InitializeInternalCollections();
        }

        public virtual User User { get; protected set; }

        private ISet<Realty> _realties;
        public virtual IEnumerable<Realty> Realties => _realties;

        public virtual string Telephone { get; set; }

        public virtual void AddRealty(Realty realty)
        {
            if (realty.Client.Id != Id)
                throw new DomainEntityException("Attempt to add a realty that does not belong to the client");

            if (_realties.Contains(realty))
                return;

            _realties.Add(realty);
        }

        public virtual void RemoveRealty(Realty realty)
        {
            if (!_realties.Contains(realty))
                return;

            _realties.Remove(realty);
        }

        protected virtual void InitializeInternalCollections()
        {
            _realties = new HashSet<Realty>();
        }
    }
}