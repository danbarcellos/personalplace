using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentValidation;
using FluentValidation.Validators;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping;
using Pactor.Infra.DAL.ORM.NHibernate.Config;

namespace Pactor.Infra.DAL.ORM.NHibernate.Facility
{
    public class ValidationConfigurator : IValidationConfigurator
    {
        private readonly IDictionary<string, List<Type>> _validatorTypes = new Dictionary<string, List<Type>>();

        public IValidationConfigurator RegisterFromAssembly(ContainerBuilder builder, Assembly assembly, Func<Type, bool> filter = null)
        {
            if (filter == null)
                filter = val => true;

            if (_validatorTypes.ContainsKey(assembly.FullName))
                return this;
            var key = assembly.FullName;
            _validatorTypes.Add(assembly.FullName, new List<Type>());

            var validators = AssemblyScanner.FindValidatorsInAssembly(assembly).Where(t => filter(t.ValidatorType));

            foreach (var validator in validators)
            {
                _validatorTypes[key].Add(validator.ValidatorType);
                builder.RegisterType(validator.ValidatorType)
                    .As(validator.InterfaceType)
                    .As<IValidator>()
                    .SingleInstance();
            }

            return this;
        }

        public IValidationConfigurator ApplyingDDLConstraints(Configuration configuration)
        {
            var rules = GetRules();
            foreach (PersistentClass entity in configuration.ClassMappings.Where(entity => rules.ContainsKey(entity.MappedClass)))
            {
                ApplyRulesToPersistentClass(entity, rules[entity.MappedClass]);
            }

            return this;
        }

        private void ApplyRulesToPersistentClass(PersistentClass entity, ILookup<string, IPropertyValidator> propertyRules)
        {
            foreach (var propertyRule in propertyRules)
            {
                var property = FindPropertyByName(entity, propertyRule.Key);
                if (property == null)
                    continue;

                foreach (var propertyValidator in propertyRule)
                {
                    var validatorType = propertyValidator.GetType();

                    if (typeof(ILengthValidator).IsAssignableFrom(validatorType))
                    {
                        if (propertyValidator is ILengthValidator validator && validator.Max < int.MaxValue)
                        {
                            IEnumerator ie = property.ColumnIterator.GetEnumerator();
                            ie.MoveNext();
                            var col = (Column)ie.Current;
                            col.Length = validator.Max;
                        }
                        continue;
                    }

                    if (typeof(INotNullValidator).IsAssignableFrom(validatorType))
                    {
                        //single table should not be forced to null
                        if (!property.IsComposite && !(property.PersistentClass is SingleTableSubclass))
                        {
                            foreach (var column in property.ColumnIterator.Cast<Column>())
                            {
                                column.IsNullable = false;
                            }
                        }
                    }
                }
            }
        }

        private static Property FindPropertyByName(PersistentClass associatedClass, string propertyName)
        {
            Property property;
            var idProperty = associatedClass.IdentifierProperty;
            var idName = idProperty?.Name;
            try
            {
                if (string.IsNullOrEmpty(propertyName) || propertyName.Equals(idName)) //if it's a Id
                {
                    property = idProperty;
                }
                else //if it's a property
                {
                    property = associatedClass.GetProperty(propertyName);
                }
            }
            catch (MappingException)
            {
                return null;
            }

            return property;
        }

        private IDictionary<Type, ILookup<string, IPropertyValidator>> GetRules()
        {
            var validators = 
                _validatorTypes.SelectMany(x => x.Value)
                               .Select(validatorType => (IValidator) Activator.CreateInstance(validatorType));

            var rules = new Dictionary<Type, ILookup<string, IPropertyValidator>>();

            foreach (var validator in validators)
            {
                var entityType = GetEntityType(validator);
                var propertyValidators = validator.CreateDescriptor().GetMembersWithValidators();
                
                rules.Add(entityType, propertyValidators);
            }

            return rules;
        }

        private static Type GetEntityType(IValidator validator)
        {
            var type = validator.GetType();
            return type.BaseType != null && type.BaseType.IsGenericType ? type.BaseType.GetGenericArguments()[0] : null;
        }
    }
}