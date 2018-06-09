using System;
using NHibernate.Engine;

namespace PersonalPlace.Domain.Base.Filters
{
    public class ScopeFilter
    {
        private readonly Action<NHibernate.IQuery, string> _setParameterDelegate;

        public ScopeFilter(string name, string parameterName, string criteria, FilterDefinition definition, Action<NHibernate.IQuery, string> setParameterDelegate)
        {
            Name = name;
            ParameterName = parameterName;
            Criteria = criteria;
            Definition = definition;
            _setParameterDelegate = setParameterDelegate;
        }

        public string Name { get; private set; }
        public string ParameterName { get; private set; }
        public string Criteria { get; private set; }
        public FilterDefinition Definition { get; private set; }

        public virtual void SetQueryParameter(NHibernate.IQuery query, string scopeTag)
        {
            _setParameterDelegate(query, scopeTag);
        }
    }
}