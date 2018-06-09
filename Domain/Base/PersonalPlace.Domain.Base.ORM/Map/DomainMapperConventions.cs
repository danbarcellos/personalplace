using System;
using System.Linq;
using System.Reflection;
using NHibernate.Mapping.ByCode;
using Pactor.Infra.DAL.ORM.NHibernate.Map;
using PersonalPlace.Domain.Base.Component;
using PersonalPlace.Domain.Base.CustomType;
using PersonalPlace.Domain.Base.Filters;

namespace PersonalPlace.Domain.Base.ORM.Map
{
    public class DomainMapperConventions : ConventionsMapper
    {
        public const string AssociationsTableIdSufix = ManyToManyIntermediateTableInfix + PrimaryKeyColumnPostfix;
        public static readonly Type BaseEntityType = typeof(ScopedEntity);
        private readonly Type[] _baseEntityTypes = { typeof(ScopedEntity), typeof(EntityWithState<>), typeof(EntityWithStaticState<>), typeof(EntityWithStateRecord<>), typeof(StateRegister<>) };

        public DomainMapperConventions()
        {
            IsEntity((t, declared) => _baseEntityTypes.All(et => et != t) && BaseEntityType.IsAssignableFrom(t));
            IsRootEntity((t, declared) => _baseEntityTypes.All(et => et != t && (!t.IsGenericType || (t.IsGenericType && t.GetGenericTypeDefinition() != et)))
                                         && _baseEntityTypes.Any(et => et == t.BaseType || (t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == et)));

            Class<Entidade>(map =>
            {
                map.Id(x => x.Id);
                map.Version("Version", m => m.Generated(VersionGeneration.Never));
            });

            Class<ScopedEntity>(map =>
            {
                map.Property(x => x.ScopeTag, m =>
                {
                    m.Length(400);
                    m.NotNullable(true);
                });
            });

            BeforeMapProperty += (mi, type, map) =>
            {
                if (type.LocalMember.Name == nameof(EntityState<object>.StateNote))
                    map.Length(400);
            };

            BeforeMapClass +=
                (mi, type, map) =>
                {
                    map.Id(idmap =>
                    {
                        idmap.Generator(Generators.GuidComb);
                        var entityNameAssociation = GetEntityNameAssociation(type);
                        idmap.Column(entityNameAssociation == null
                            ? GetPrimaryKeyColumnName(type)
                            : string.Concat(entityNameAssociation, AssociationsTableIdSufix));
                    });

                    if (!typeof(ScopedEntity).IsAssignableFrom(type) || !type.GetCustomAttributes(inherit: false).Any(attribute => attribute is EntityParameterAttribute))
                        return;

                    var entiyScopeAttribute = type.GetCustomAttributes(typeof(EntityParameterAttribute), inherit: false).Cast<EntityParameterAttribute>().Single();

                    switch (entiyScopeAttribute.Segregation)
                    {
                        case Segregation.Root:
                            map.Filter(ScopeFilters.RootSegregation.Name, x => x.Condition(ScopeFilters.RootSegregation.Criteria));
                            break;
                        case Segregation.Descending:
                            map.Filter(ScopeFilters.DescendingSegregation.Name, x => x.Condition(ScopeFilters.DescendingSegregation.Criteria));
                            break;
                        case Segregation.Ascending:
                            map.Filter(ScopeFilters.AscendingSegregation.Name, x => x.Condition(ScopeFilters.AscendingSegregation.Criteria));
                            break;
                        case Segregation.Parenthood:
                            map.Filter(ScopeFilters.ParenthoodSegregation.Name, x => x.Condition(ScopeFilters.ParenthoodSegregation.Criteria));
                            break;
                    }
                };

            BeforeMapOneToMany += (mi, member, map) =>
            {
                var type = member.LocalMember.ReflectedType;
                var baseType = type?.BaseType;
                var collectionType = member.CollectionElementType();

                if (type == null
                    || (baseType == null || !baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(EntityWithStateRecord<>))
                    || (!collectionType.IsGenericType || collectionType.GetGenericTypeDefinition() != typeof(StateRegister<>)))
                    return;

                var registroEstadoType = GetStateRecordType(type.BaseType);
                
                if (registroEstadoType == null) 
                    return;
                
                map.Class(registroEstadoType);
            };

            BeforeMapSet += 
                (mi, member, map) =>
                {
                    var type = member.LocalMember.ReflectedType;
                    var baseType = type?.BaseType;
                    var collectionType = member.CollectionElementType();

                    if (type == null
                        || (baseType == null || !baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(EntityWithStateRecord<>))
                        || (!collectionType.IsGenericType || collectionType.GetGenericTypeDefinition() != typeof(StateRegister<>)))
                        return;

                    var orderMember = member.CollectionElementType().GetProperty("StateDateTime");
                    map.Key(km => km.Column(string.Concat(type.Name, PrimaryKeyColumnPostfix)));
                    map.OrderBy(orderMember);
                    map.Fetch(CollectionFetchMode.Select);
                };
            BeforeMapProperty += BeforeMapPropertyConvention;
            AfterMapClass += BeforeMapAssociationsEntityNamingConventions;
        }

        public static string GetPrimaryKeyColumnName(Type type)
        {
            return type.Name + PrimaryKeyColumnPostfix;
        }

        public static string GetEntityNameAssociation(Type associacaoType)
        {
            if (associacaoType == null)
                throw new ArgumentNullException(nameof(associacaoType));

            var typeName = associacaoType.Name;

            if (!(typeName.StartsWith("Associacao") || typeName.EndsWith("Association")))
                return null;

            var fromEntityProperty = associacaoType.GetProperties().FirstOrDefault(x => typeName.Contains(x.PropertyType.Name));

            if (fromEntityProperty == null)
                return null;

            var fromEntityName = fromEntityProperty.Name;

            var toEntityProperty = associacaoType.GetProperties().FirstOrDefault(x => typeName.Contains(x.PropertyType.Name) && x.PropertyType.Name != fromEntityName);

            if (toEntityProperty == null)
                return null;

            var toEntityName = toEntityProperty.Name;

            return string.Join(ManyToManyIntermediateTableInfix, fromEntityName, toEntityName);
        }

        private static Type GetStateRecordType(Type registroEstadoType)
        {
            var tipoEstadoType = registroEstadoType.GetGenericArguments()[0];
            return StateRegisterFactory.GetStateRecordType(tipoEstadoType);
        }

        private static void BeforeMapPropertyConvention(IModelInspector modelinspector, PropertyPath member, IPropertyMapper propertycustomizer)
        {
            if (member.LocalMember.GetCustomAttribute<EncryptedStringAttribute>() != null)
                propertycustomizer.Type<EncryptedString>();
        }

        private static void BeforeMapAssociationsEntityNamingConventions(IModelInspector modelinspector, Type type, IClassAttributesMapper classcustomizer)
        {
            string entityName;
            if ((entityName = GetEntityNameAssociation(type)) == null)
                return;

            classcustomizer.Table(entityName);
            classcustomizer.Id(map =>
            {
                map.Column(string.Concat(entityName, AssociationsTableIdSufix));
                map.Generator(Generators.GuidComb);
            });
        }
    }
}