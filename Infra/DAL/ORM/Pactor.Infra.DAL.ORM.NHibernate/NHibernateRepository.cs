using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using NHibernate;
using Pactor.Infra.Crosscutting.Exceptions;
using Pactor.Infra.Crosscutting.Log;
using Pactor.Infra.DAL.ORM.Queries;
using IQuery = Pactor.Infra.DAL.ORM.Queries.IQuery;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public class NHibernateRepository<T> : NHibernateBase, IRepository<T> where T : Entity
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IValidatorFactory _validatorFactory;
        private readonly IEnumerable<IPreInsertInterceptor> _preInsertInterceptors;

        private readonly ILog _logger;

        public NHibernateRepository(ISession session,
                                    IQueryFactory queryFactory,
                                    IValidatorFactory validatorFactory,
                                    IEnumerable<IPreInsertInterceptor> preInsertInterceptors,
                                    ILog logger)
            : base(session)
        {
            _queryFactory = queryFactory;
            _logger = logger;
            _validatorFactory = validatorFactory;
            _preInsertInterceptors = preInsertInterceptors;
        }

        public void SaveAll<TEntity>(TEntity entity, string memberName = null, int sourceLineNumber = 0) where TEntity : class, T
        {
            SaveEntity(entity, false, memberName, sourceLineNumber);
        }

        public void Save<TEntity>(TEntity entity, [CallerMemberName] string memberName = null, [CallerLineNumber] int sourceLineNumber = 0) where TEntity : class, T
        {
            SaveEntity(entity, true, memberName, sourceLineNumber);
        }

        private void SaveEntity<TEntity>(TEntity entity, bool saveOnly, string memberName = null, int sourceLineNumber = 0) where TEntity : T
        {
            try
            {
                var validator = GetValidator<TEntity>();
                if (validator != null)
                {
                    var validationResult = validator.Validate(entity);

                    if (!validationResult.IsValid)
                    {
                        if (_logger.IsErrorEnabled)
                        {
                            var invalidValuesString = validationResult.Errors
                                .Select(x => $"{typeof(T).Name}.{x.PropertyName} = {x.AttemptedValue}\nCause: {x.ErrorMessage}")
                                .Aggregate(new StringBuilder(),
                                    (sb, invalidValueMessage) => sb.AppendLine(invalidValueMessage))
                                .ToString();

                            _logger.ErrorFormat($"Invalid state of entity {entity.GetType().Name}: \n{invalidValuesString} \n\nSource: {memberName} at {sourceLineNumber}");
                        }

                        var invalidValues = GetInvalidValues(entity, validationResult);
                        throw new InvalidStateException(invalidValues);
                    }
                }
            }
            catch (InvalidStateException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(() => $"Unknown validation error. Entity: {nameof(T)} [{entity.Id}]\nEntity\nDetails: {e.InnerException?.Message ?? "None"} \n\nSource: {memberName} at {sourceLineNumber}", e);
                throw;
            }

            try
            {
                foreach (var preInsertInterceptor in _preInsertInterceptors)
                {
                    var result = preInsertInterceptor.OnPreInsert(entity);

                    if (result.Proceed)
                        continue;

                    var message = $"Entity insert refused by interceptor \"{preInsertInterceptor.Description}\". Entity: {entity.GetType().Name}. Fail message: {result.FailMessage}";
                    _logger.Error(() => message, result.Exception);
                    throw new InterceptorException(result.FailMessage, result.Exception);
                }

                if (saveOnly)
                    Transact(() => Session.Save(entity));
                else
                    Transact(() => Session.SaveOrUpdate(entity));
            }
            catch (global::NHibernate.StaleObjectStateException e)
            {
                _logger.Error(() => $"Stale object state error. Entity: {entity.GetType().Name} [{(entity.Id == Entity.UnsavedId ? "new" : entity.Id.ToString())}]\nDetails: {e.InnerException?.Message ?? "None"}", e);

                throw new StaleObjectStateException(e.Message, e.EntityName, e.Identifier, e);
            }
        }

        public bool Contains(T item)
        {
            if (item.Id == Entity.UnsavedId)
                return false;

            return Transact(() => Session.Get<T>(item.Id)) != null;
        }

        public int Count
        {
            get
            {
                return Transact(() => Session.Query<T>().Count());
            }
        }

        public bool Remove(T item)
        {
            Transact(() => Session.Delete(item));
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Transact(() => Session.Query<T>().Take(1000).GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Transact(() => GetEnumerator());
        }

        public TQuery CreateQuery<TQuery>() where TQuery : IQuery
        {
            return _queryFactory.CreateQuery<TQuery>();
        }

        public InvalidValue[] GetInvalidValues<TEntity>(TEntity item) where TEntity : T
        {
            var validator = GetValidator<TEntity>();
            var validationResult = validator.Validate(item);
            return !validationResult.IsValid ? GetInvalidValues(item, validationResult) : new InvalidValue[] { };
        }

        public T Load(object id)
        {
            return Session.Load<T>(id);
        }

        public TEntity FindOne<TEntity>(object id) where TEntity : class, T
        {
            return Transact(() => Session.Get<TEntity>(id));
        }

        public T FindOne(object id)
        {
            return Transact(() => Session.Get<T>(id));
        }

        private IValidator GetValidator<TEntity>()
        {
            return _validatorFactory.GetValidator<TEntity>();
        }

        private static InvalidValue[] GetInvalidValues(T item, ValidationResult validationResult)
        {
            var errors = validationResult.Errors;
            var invalidValues = errors.Select(e =>
                new InvalidValue
                {
                    Entity = item,
                    EntityType = typeof(T),
                    Message = e.ErrorMessage,
                    Value = e.AttemptedValue
                });

            return invalidValues.ToArray();
        }
    }
}