using System;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Base.Component;
using PersonalPlace.Domain.Common.ValueObjects;

namespace PersonalPlace.Domain.Entities.Catalog
{
    [EntityParameter(Segregation.Descending, isAggregateRoot: true)]
    public class Comment : EntityWithStateRecord<CommentsStateType>
    {
        protected Comment()
        {
            InitializeInternalCollections();
        }

        public Comment(Realty realty, Client client, Comment mention = null, DateTimeOffset? dateTime = null, string scopeTag = null, CommentsStateType? stateType = null)
            : this(realty, client, mention, dateTime ?? DateTimeOffset.Now, scopeTag ?? client.ScopeTag, new EntityState<CommentsStateType>(stateType ?? CommentsStateType.Created))
        {
        }

        public Comment(Realty realty, Client client, Comment mention, DateTimeOffset dateTime, string scopeTag, EntityState<CommentsStateType> state) : base(state, scopeTag)
        {
            Realty = realty;
            Client = client;
            Mention = mention;
            DateTime = dateTime;
        }

        public virtual Realty Realty { get; protected set; }

        public virtual Client Client { get; protected set; }

        public virtual Comment Mention { get; protected set; }

        public virtual DateTimeOffset DateTime { get; protected set; }

        public virtual string Text { get; set; }
    }
}