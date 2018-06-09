using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentValidation.Results;
using NHibernate;
using Pactor.Infra.Crosscutting.Exceptions;
using Pactor.Infra.Crosscutting.IoC;
using Pactor.Infra.DAL.ORM.NHibernate.Query;
using Pactor.Infra.DAL.ORM.Queries;
using IQuery = Pactor.Infra.DAL.ORM.Queries.IQuery;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class NHStatelessUnitOfWork : NHibernateStatelessBase, IStatelessUnitOfWork
    {
        private readonly IContainer _container;
        private readonly IValidatorFactory _validatorFactory;
        private readonly IEnumerable<IPreInsertInterceptor> _preInsertInterceptors;
        private readonly IDisposable _sessionDisposable;
        
        public NHStatelessUnitOfWork(IStatelessSession session, 
                                     IContainer container, 
                                     IValidatorFactory validatorFactory,
                                     IEnumerable<IPreInsertInterceptor> preInsertInterceptors, 
                                     IDisposable sessionDisposable = null)
            : base(session)
        {
            _container = container;
            _validatorFactory = validatorFactory;
            _preInsertInterceptors = preInsertInterceptors;
            _sessionDisposable = sessionDisposable;
        }

        public IDbConnection Connection
        {
            get { return Session.Connection; }
        }

        public bool IsConnected
        {
            get { return Session.IsConnected; }
        }

        public bool IsOpen
        {
            get { return Session.IsOpen; }
        }

        public void Commit()
        {
            if (!Session.IsOpen)
                return;

            if (Session.Transaction.IsActive)
                Session.Transaction.Commit();
        }

        public ITransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return new NHTransaction(Session.Transaction.IsActive ? Session.Transaction : Session.BeginTransaction(isolation));
        }

        public T Get<T>(object id)
        {
            return Transact(() => Session.Get<T>(id));
        }

        public void Insert(object entity)
        {
            RunInterceptors(entity as Entity);
            Transact(() => Session.Insert(entity));
        }

        public void Update(object entity)
        {
            RunInterceptors(entity as Entity);
            Transact(() => Session.Update(entity));
        }

        public void Delete(object entity)
        {
            Transact(() => Session.Delete(entity));
        }

        public void Refresh(object entity)
        {
            Transact(() => Session.Refresh(entity));
        }

        public void Close()
        {
            Session.Close();
        }

        public void SetBatchSize(int batchSize)
        {
            Session.SetBatchSize(batchSize);
        }

        public TQuery CreateQuery<TQuery>() where TQuery : IQuery
        {
            var query = _container.Resolve<TQuery>(new TypedParameter(typeof(IQueryMachine), new StateLessQueryMachine(Session)));

            var queryConstraint = query.GetType().GetCustomAttributes(typeof(QueryConstraintAttribute), false);
            if (queryConstraint.Any() && !((QueryConstraintAttribute)queryConstraint[0]).StatelessEnable)
                throw new InvalidOperationException("This query can not be performed by a stateless unit of work");

            return query;
        }

        public InvalidValue[] GetInvalidValues<TEntity>(TEntity item)
        {
            var validator = _validatorFactory.GetValidator<TEntity>();
            
            if (validator == null) 
                return new InvalidValue[] {};

            var validationResult = validator.Validate(item);
            return !validationResult.IsValid ? GetInvalidValues(item, validationResult) : new InvalidValue[] {};
        }

        public void Flush()
        {
            Session.GetSessionImplementation().Flush();
        }

        private void RunInterceptors<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity == null)
                return;

            foreach (var preInsertInterceptor in _preInsertInterceptors)
            {
                var result = preInsertInterceptor.OnPreInsert(entity);

                if (result.Proceed)
                    continue;
                
                throw new InterceptorException(result.FailMessage, result.Exception);
            }
        }
        
        private static InvalidValue[] GetInvalidValues<TEntity>(TEntity item, ValidationResult validationResult)
        {
            var invalidValues = validationResult.Errors.Select(x => new InvalidValue
            {
                Entity = item,
                EntityType = typeof(TEntity),
                Message = x.ErrorMessage,
                PropertyName = x.PropertyName,
                Value = x.AttemptedValue
            }).ToArray();

            return invalidValues;
        }

        public void Dispose()
        {
            Session.Dispose();
            _sessionDisposable?.Dispose();
        }
    }
}
