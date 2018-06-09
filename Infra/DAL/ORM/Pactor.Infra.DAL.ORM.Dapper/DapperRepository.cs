using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using FluentValidation;
using FluentValidation.Results;
using Pactor.Infra.Crosscutting.Exceptions;
using Pactor.Infra.Crosscutting.Log;

namespace Pactor.Infra.DAL.ORM.Dapper
{
    public class DapperRepository<T> : IRepository<T> where T : class, IEntity
    {
        private static readonly PropertyInfo IdProperty = typeof(T).GetProperty(nameof(IEntity<object>.Id), BindingFlags.Instance | BindingFlags.Public);
        private static readonly PropertyInfo RowVersionProperty = typeof(T).GetProperty(OptmisticVersionColumnName, BindingFlags.Instance | BindingFlags.NonPublic);
        private const string OptmisticVersionColumnName = "Version";

        private readonly ILog _logger;
        private readonly IDbConnection _dbConnection;
        private readonly ISqlGenerator _sqlGenerator;
        private readonly IValidatorFactory _validatorFactory;
        // ReSharper disable StaticMemberInGenericType
        private static string _insertSql;
        private static string _updateSql;
        // ReSharper restore StaticMemberInGenericType


        public DapperRepository(ILog logger,
                                IDbConnection dbConnection,
                                ISqlGenerator sqlGenerator,
                                IValidatorFactory validatorFactory)
        {
            if (_insertSql == null)
                CreateSQL();

            _logger = logger;
            _dbConnection = dbConnection;
            _sqlGenerator = sqlGenerator;
            _validatorFactory = validatorFactory;
        }

        public int Count => _dbConnection.Count<T>(null);

        public bool Contains(T entity)
        {
            var id = GetId(entity);
            return _dbConnection.Get<T>(id) != null;
        }

        public T FindOne(object id)
        {
            return _dbConnection.Get<T>(id);
        }

        public TEntity FindOne<TEntity>(object id) where TEntity : class, T
        {
            return _dbConnection.Get<TEntity>(id);
        }

        public T Load(object id)
        {
            return _dbConnection.Get<T>(id);
        }

        public void SaveAll<TEntity>(TEntity entity, string memberName = null, int sourceLineNumber = 0) where TEntity : class, T
        {
            throw new NotImplementedException();
        }

        public void Save<TEntity>(TEntity entity, string memberName = null, int sourceLineNumber = 0) where TEntity : class, T
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityId = GetId(entity);

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
                            var invalidValuesString =
                                validationResult.Errors
                                                .Select(x => $"{typeof(T).Name}.{x.PropertyName} = {x.AttemptedValue}\nCause: {x.ErrorMessage}")
                                                .Aggregate(new StringBuilder(), (sb, invalidValueMessage) => sb.AppendLine(invalidValueMessage))
                                                .ToString();

                            _logger.Error(() => $"Invalid state of entity {entity.GetType().Name}: \n{invalidValuesString} \n\nSource: {memberName} at {sourceLineNumber}");
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
                _logger.Error(() => $"Unknown validation error. Entity: {nameof(T)} [{entityId}]\nEntity\nDetails: {e.InnerException?.Message ?? "None"} \n\nSource: {memberName} at {sourceLineNumber}", e);
                throw;
            }

            try
            {
                if ((Guid)entityId == default(Guid))
                {
                    _dbConnection.Execute(_insertSql, entity);
                }
                else
                {
                    var totalLines = _dbConnection.Execute(_insertSql, entity);

                    if (RowVersionProperty != null && TryGetVersion(entity, out var currentVersion))
                        RowVersionProperty.SetValue(entity, currentVersion);

                    if (totalLines < 1)
                        throw new StaleObjectStateException("Row was updated or deleted by another transaction", nameof(T), entityId);
                }
            }
            catch (SqlException e)
            {
                var translatedException = TranslateSqlException(e, entityId);
                throw translatedException;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Remove(T entity)
        {
            return _dbConnection.Delete(entity);
        }

        public InvalidValue[] GetInvalidValues<TEntity>(TEntity entity) where TEntity : T
        {
            var validator = GetValidator<TEntity>();
            var validationResult = validator.Validate(entity);
            return !validationResult.IsValid ? GetInvalidValues(entity, validationResult) : new InvalidValue[] { };
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        private void CreateSQL()
        {
            var sbFields = new StringBuilder();
            var sbVariables = new StringBuilder();
            var sbFieldValues = new StringBuilder();
            var classMap = _sqlGenerator.Configuration.GetMap<T>();
            var idProperty = classMap.Properties.First(x => x.KeyType != KeyType.NotAKey);

            foreach (var property in classMap.Properties)
            {
                if (property != idProperty)
                {
                    sbFieldValues.Append(property.PropertyInfo == RowVersionProperty
                        ? $",[{property.ColumnName}] = @{property.Name} + 1"
                        : $",[{property.ColumnName}] = @{property.Name}");
                }
                else if (property.KeyType != KeyType.Assigned && property.KeyType != KeyType.Guid)
                {
                    continue;
                }

                sbFields.Append($",[{property.ColumnName}]");
                sbVariables.Append($",@{property.Name}");
            }

            var variables = sbVariables.ToString(1, sbVariables.Length - 1);
            var fields = sbFields.ToString(1, sbFields.Length - 1);
            var fieldValues = sbFieldValues.ToString(1, sbFieldValues.Length - 1);

            _insertSql = $"INSERT INTO [{classMap.TableName}] ({fields}) VALUES ({variables})";
            _updateSql = $"UPDATE [{classMap.TableName}] SET {fieldValues} WHERE [{idProperty.ColumnName}] = @{idProperty.Name}";

            if (RowVersionProperty != null)
                _updateSql = $"{_updateSql} AND [{OptmisticVersionColumnName}] = @{OptmisticVersionColumnName}";
        }

        private object GetId(T entity)
        {
            return IdProperty.GetValue(entity);
        }

        private bool TryGetVersion(T entity, out int version)
        {
            if (_validatorFactory == null)
            {
                version = default(int);
                return false;
            }

            version = (int)RowVersionProperty.GetValue(entity);
            return true;
        }

        private Exception TranslateSqlException(SqlException sqlException, object entityId)
        {
            switch (sqlException.Number)
            {
                case 547:
                    return new ConstraintViolationException(sqlException.Message, sqlException.InnerException, string.Empty);
                case 3960:
                    return new StaleObjectStateException(nameof(T), entityId);
                case 1205:
                    return new DeadlockException(sqlException.Message, nameof(T), entityId, sqlException.InnerException);
                default:
                    return sqlException;
            }
        }
    }
}
