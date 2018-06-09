using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Entities.Properties;
using PersonalPlace.Domain.Entities.Structure;

namespace PersonalPlace.Domain.Entities.Security
{
    [EntityParameter(Segregation.Parenthood)]
    public class Authorization : ScopedEntity
    {
        static readonly Regex GuidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

        protected Authorization()
        {
        }

        public Authorization(User user, Unit unit, string[] roles = null, string scopeTag = null) : base(scopeTag ?? unit.ScopeTag)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            User = user;
            Unit = unit;

            if (roles == null)
                _roles.Add(Common.RBAC.Roles.User);
            else
            {
                _roles.AddRange(roles);

                if (!_roles.Contains(Common.RBAC.Roles.User))
                    _roles.Add(Common.RBAC.Roles.User);
            }

        }

        public virtual User User { get; protected set; }

        public virtual Unit Unit { get; protected set; }

        public virtual DateTime? ExpirationDate { get; set; }

        protected virtual string RolesPersistent
        {
            get => string.Join(",", _roles);
            set => _roles = GetRolesArray(value).ToList();
        }

        private List<string> _roles = new List<string>();
        public virtual IEnumerable<string> Roles => _roles;

        public virtual void AssignRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));

            if (!GuidRegEx.IsMatch(role))
                throw new ArgumentNullException(nameof(role), Resources.Authorization_AssignRole_The_role_key_has_not_a_compatible_format_with_a_GUID);

            if (!_roles.Contains(role))
                _roles.Add(role);
        }

        public virtual void AssignNewRoleSet(string[] roles)
        {
            if (roles == null)
                throw new ArgumentNullException(nameof(roles));

            _roles = new List<string>(roles) { Common.RBAC.Roles.User };
        }

        public virtual void RemoveRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));

            if (!GuidRegEx.IsMatch(role))
                throw new ArgumentNullException(nameof(role), Resources.Authorization_AssignRole_The_role_key_has_not_a_compatible_format_with_a_GUID);

            if (_roles.Contains(role))
                _roles.Remove(role);
        }

        public virtual bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        public static string[] GetRolesArray(string roleString)
        {
            return string.IsNullOrWhiteSpace(roleString)
                ? new string[] { }
                : roleString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}