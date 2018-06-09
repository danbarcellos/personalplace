using System;
using System.Data.SqlClient;
using NHibernate.Exceptions;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class NHMsSqlExceptionConverter : ISQLExceptionConverter
    {
        public Exception Convert(AdoExceptionContextInfo exInfo)
        {
            var sqle = ADOExceptionHelper.ExtractDbException(exInfo.SqlException) as SqlException;
            if (sqle != null)
            {
                switch (sqle.Number)
                {
                    case 547:
                        return new ConstraintViolationException(exInfo.Message, sqle.InnerException, exInfo.Sql);
                    case 208:
                        return new SQLGrammarException(exInfo.Message, sqle.InnerException, exInfo.Sql);
                    case 3960:
                        return new StaleObjectStateException(exInfo.EntityName, exInfo.EntityId);
                    case 1205:
                        return new DeadlockException(sqle.Message, exInfo.EntityName, exInfo.EntityId, sqle.InnerException);
                }
            }

            return SQLStateConverter.HandledNonSpecificException(exInfo.SqlException, exInfo.Message, exInfo.Sql);
        }
    }
}