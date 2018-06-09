using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Impl;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public static class QueryMachineExtensions
    {
        public static bool HasMultipleQueriesInProgress(this AbstractSessionImpl sessionImpl)
        {
            var fieldInfo = typeof(FutureBatch<IQuery, IMultiQuery>).GetField("queries", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
                throw new Exception("Error checking multiple queries in progress. Possible incompatible NHibernate version");

            var queries = (IEnumerable<IQuery>)fieldInfo.GetValue(sessionImpl.FutureQueryBatch);
            return queries.Any();
        }
    }
}