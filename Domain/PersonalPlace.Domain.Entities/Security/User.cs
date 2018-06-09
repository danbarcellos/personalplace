using System;
using System.Collections.Generic;
using System.Linq;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Base.Component;
using PersonalPlace.Domain.Common.Exception;
using PersonalPlace.Domain.Common.ValueObjects;

namespace PersonalPlace.Domain.Entities.Security
{
    [EntityParameter(Segregation.Parenthood, isAggregateRoot: true)]
    public class User : EntityWithStaticState<UserStateType>
    {
        protected User()
        {
            InitializeInternalCollections();
        }

        public User(string firstName, string lastName, string email, string password, UserStateType state = UserStateType.Created, string scopeTag = null)
            : this(firstName, lastName, email, password, new EntityState<UserStateType>(state), scopeTag)
        {
        }

        public User(string firstName, string lastName, string email, string password, EntityState<UserStateType> state, string scopeTag = null) : base(state, scopeTag ?? DomainContextConstants.RootScopeTag)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            InitializeInternalCollections();
        }

        public virtual string ExternalId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; protected set; }

        public virtual string Password { get; protected set; }
        
        public virtual string PhotoFilePrefix { get; set; }

        private ISet<Authorization> _authorizations;
        public virtual IEnumerable<Authorization> Authorizations => _authorizations;

        private ISet<UserProperty> _properties;
        public virtual IEnumerable<UserProperty> Properties => _properties;

        public virtual bool ChangePasswordRequired { get; set; }

        public virtual void AddAuthorization(Authorization authorization)
        {
            if (authorization.User != this)
                throw new DomainArgumentException("Attempting to add an authorization that does not belong to the user");

            var currentAuthorization = _authorizations.SingleOrDefault(x => x.Unit == authorization.Unit);
            if (currentAuthorization == null)
            {
                _authorizations.Add(authorization);
            }
            else
            {
                _authorizations.Remove(currentAuthorization);
                _authorizations.Add(authorization);
            }
        }

        public virtual void RemoveAuthorization(Authorization authorization)
        {
            if (authorization.User != this)
                throw new DomainArgumentException("Attempting to remove an authorization that does not belong to the user");

            var currentAuthorization = _authorizations.SingleOrDefault(x => x.Unit == authorization.Unit);
            if (currentAuthorization == null)
            {
                _authorizations.Remove(authorization);
            }
        }

        public virtual void AssignProperty(string key, string value)
        {
            var property = _properties.SingleOrDefault(x => x.Key == key);
            if (property == null)
            {
                _properties.Add(new UserProperty(this, key)
                {
                    Value = value
                });
            }
            else
            {
                property.Value = value;
            }
        }

        public virtual void RemoveProperty(string key)
        {
            var property = _properties.SingleOrDefault(x => x.Key == key);
            if (property != null)
            {
                _properties.Remove(property);
            }
        }

        public virtual void ClearProperties()
        {
            _properties.Clear();
        }

        public virtual void ChangePassword(string currentPassword, string newPassword, bool changePasswordRequired = false)
        {
            if (Password != currentPassword)
                throw new DomainArgumentException("Error trying to change the password. Invalid current password.");

            Password = newPassword;
            ChangePasswordRequired = changePasswordRequired;
        }


        protected override void InitializeInternalCollections()
        {
            base.InitializeInternalCollections();
            _authorizations = new HashSet<Authorization>();
            _properties = new HashSet<UserProperty>();
        }
    }
}