using PersonalPlace.Domain.Base;

namespace PersonalPlace.Domain.Entities.Security
{
    public class UserProperty : ScopedEntity
    {
        protected UserProperty()
        {
        }

        public UserProperty(User user, string key, string scopeTag = null) : base(scopeTag ?? user.ScopeTag)
        {
            User = user;
            Key = key;
        }

        public virtual User User { get; protected set; }

        public virtual string Key { get; protected set; }

        public virtual string Value { get; set; }
    }
}