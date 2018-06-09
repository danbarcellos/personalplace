using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace PersonalPlace.Domain.Base
{
    public class DomainContext : IDomainContext
    {
        public const string SystemInstance = "SystemInstance";
        public const string UserInstance = "UserInstance";

        public DomainContext(Guid userId, string token, string userName, string fullName, string scopeTag, string culture, string[] roles, IDictionary<string, string> properties)
        {
            ClientToken = token;
            ScopeTag = scopeTag;
            Culture = culture;
            UserContext = new UserContextImpl(userId, userName, fullName, roles, properties);
        }

        public static DomainContext Current => OperationContext.Current?.Extensions.Find<DomainContext>();

        public static DomainContext GetDefault()
        {
            return new DomainContext(DomainContextConstants.DefaultId,
                                     DomainContextConstants.DefaultToken,
                                     DomainContextConstants.DefaultLogin,
                                     DomainContextConstants.DefaultName,
                                     DomainContextConstants.DefaultScopeTag,
                                     DomainContextConstants.DefaultCulture,
                                     new string[] {},
                                     new Dictionary<string, string>());
        }

        public string ClientToken { get; }

        public string ScopeTag { get; }

        public string Culture { get; }

        public IUserContext UserContext { get; }

        public void Attach(OperationContext owner) { }

        public void Detach(OperationContext owner) { }

        private class UserContextImpl : IUserContext
        {
            private readonly string[] _roles;

            public UserContextImpl(Guid id, string login, string name, string[] roles, IDictionary<string, string> propperties)
            {
                _roles = roles;
                Id = id;
                Login = login;
                Name = name;
                Properties = new ReadOnlyDictionary<string, string>(propperties);
            }

            public Guid Id { get; }

            public string Login { get; }

            public string Name { get; }

            public ReadOnlyDictionary<string, string> Properties { get; }

            public bool IsInRole(string role)
            {
                return Id == DomainContextConstants.DefaultId || _roles.Contains(role);
            }
        }
    }
}