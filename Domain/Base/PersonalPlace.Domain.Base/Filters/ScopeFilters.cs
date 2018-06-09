using System.Collections.Generic;
using System.Collections.ObjectModel;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Type;

namespace PersonalPlace.Domain.Base.Filters
{
    public static class ScopeFilters
    {
        static ScopeFilters()
        {
            RegisteredFilters.Add(RootSegregationFilter);
            RegisteredFilters.Add(DescendingSegregationFilter);
            RegisteredFilters.Add(AscendingSegregationFilter);
            RegisteredFilters.Add(ParenthoodSegregationFilter);
        }

        private static readonly IList<ScopeFilter> RegisteredFilters = new List<ScopeFilter>();
        public static IReadOnlyCollection<ScopeFilter> Filters => new ReadOnlyCollection<ScopeFilter>(RegisteredFilters);

        #region  Root segregation filter

        // Root segregation filter
        private const string RootSegregationFilterName = "RootSegregationFilter";
        private const string RootSegregationFilterParameter = "rootScopeTag";
        private const string RootSegregationFilterCriteria = "ScopeTag LIKE :" + RootSegregationFilterParameter;

        private static readonly ScopeFilter RootSegregationFilter =
            new ScopeFilter(RootSegregationFilterName,
                            RootSegregationFilterParameter,
                            RootSegregationFilterCriteria,
                            new FilterDefinition(RootSegregationFilterName, null, new Dictionary<string, IType> {{RootSegregationFilterParameter, NHibernateUtil.String}}, useManyToOne: false),
                            (query, scopeTag) =>
                            {
                                var rootScopeTag = GetRootScopeTag(scopeTag);
                                query.SetString(RootSegregationFilterParameter, rootScopeTag + "%");
                            });

        public static ScopeFilter RootSegregation => RootSegregationFilter;

        public static string GetRootScopeTag(string scopeTag)
        {
            var pointPosition = scopeTag.IndexOf('.');
            return pointPosition == -1 ? "." : scopeTag.Substring(0, pointPosition + 1);
        }
        
        #endregion

        #region Descending segregation filter

        // Descending segregation filter
        private const string DescendingSegregationFilterName = "DescendingSegregationFilter";
        private const string DescendingSegregationFilterParameter = "scopeTag";
        private const string DescendingSegregationFilterCriteria = "ScopeTag LIKE :" + DescendingSegregationFilterParameter;

        private static readonly ScopeFilter DescendingSegregationFilter =
            new ScopeFilter(DescendingSegregationFilterName,
                            DescendingSegregationFilterParameter,
                            DescendingSegregationFilterCriteria,
                            new FilterDefinition(DescendingSegregationFilterName, null, new Dictionary<string, IType> {{DescendingSegregationFilterParameter, NHibernateUtil.String}}, useManyToOne: false),
                            (query, scopeTag) => query.SetString(DescendingSegregationFilterParameter, scopeTag));

        public static ScopeFilter DescendingSegregation => DescendingSegregationFilter;

        #endregion

        #region Ascending segregation filter

        // Ascending segregation filter
        private const string AscendingSegregationFilterName = "AscendingSegregationFilter";
        private const string AscendingSegregationFilterParameter = "ascendingScopeTags";
        private const string AscendingSegregationFilterCriteria = "ScopeTag IN(:" + AscendingSegregationFilterParameter + ")";

        private static readonly ScopeFilter AscendingSegregationFilter =
            new ScopeFilter(AscendingSegregationFilterName,
                            AscendingSegregationFilterParameter,
                            AscendingSegregationFilterCriteria,
                            new FilterDefinition(AscendingSegregationFilterName, null, new Dictionary<string, IType> {{AscendingSegregationFilterParameter, NHibernateUtil.String}}, useManyToOne: false),
                            (query, scopeTag) => query.SetParameterList(AscendingSegregationFilterParameter, scopeTag.ToAscendingScopeTags()));


        public static ScopeFilter AscendingSegregation => AscendingSegregationFilter;
        
        #endregion

        #region Parenthood segregation filter

        // Parenthood segregation filter
        private const string ParenthoodSegregationFilterName = "ParenthoodSegregationFilter";
        private const string ParenthoodSegregationFilterParameter = "parenthoodAscendingScopeTags";
        private const string ParenthoodSegregationFilterCriteria = "(ScopeTag LIKE :" + DescendingSegregationFilterParameter + " OR ScopeTag IN(:" + ParenthoodSegregationFilterParameter + "))";

        private static readonly ScopeFilter ParenthoodSegregationFilter =
            new ScopeFilter(ParenthoodSegregationFilterName,
                            ParenthoodSegregationFilterParameter,
                            ParenthoodSegregationFilterCriteria,
                            new FilterDefinition(ParenthoodSegregationFilterName, null, new Dictionary<string, IType> { { DescendingSegregationFilterParameter, NHibernateUtil.String }, { ParenthoodSegregationFilterParameter, NHibernateUtil.String } }, useManyToOne: false),
                            (query, scopeTag) =>
                            {
                                query.SetString(DescendingSegregationFilterParameter, scopeTag);
                                query.SetParameterList(ParenthoodSegregationFilterParameter, scopeTag.ToParenthoodAscendingScopeTags());
                            });


        public static ScopeFilter ParenthoodSegregation => ParenthoodSegregationFilter;

        #endregion
    }
}