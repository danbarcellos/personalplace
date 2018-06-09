﻿using NHibernate;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using PersonalPlace.Domain.Base;

namespace Pactor.Infra.DAL.ORM.NHibernateQuery
{
    public static class QueryMachineExtension
    {
        public static IDomainContext GetDomainContext(this IQueryMachine queryMachine)
        {
            var session = queryMachine.GetInnerSession<ISession>();

            if (session != null)
                return SessionDomainContextHandler.GetSessionContext(session);

            var statelessSession = queryMachine.GetInnerSession<IStatelessSession>();
            return statelessSession != null ? SessionDomainContextHandler.GetSessionContext(statelessSession) : null;
        }
    }
}