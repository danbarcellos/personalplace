using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace Pactor.Infra.DAL.ORM.NHibernate.Map
{
    public class ConventionsMapper : ConventionModelMapper
    {
        public const string PrimaryKeyColumnPostfix = "ID";
        public const string ManyToManyIntermediateTableInfix = "_";

        private const string PersistentPostfix = "Persistent";
        private const string ComponentNamespace = "Component";
        private const string ForeignKeyColumnPostfix = PrimaryKeyColumnPostfix;
        private const char ElementColumnTrimmedPluralPostfix = 's';
        private readonly IList<MemberInfo> _ignoredMembers = new List<MemberInfo>();

        public ConventionsMapper()
        {
            BeforeMapClass += BeforeMapClassConvention;
            BeforeMapManyToMany += (inspector, member, customizer) => customizer.Column(string.Concat(member.CollectionElementType().Name, ForeignKeyColumnPostfix));
            BeforeMapElement += (inspector, member, customizer) => customizer.Column(member.LocalMember.Name.TrimEnd(ElementColumnTrimmedPluralPostfix));
            BeforeMapJoinedSubclass += (inspector, type, customizer) => customizer.Key(k => k.Column(string.Concat(type.BaseType?.Name, ForeignKeyColumnPostfix)));
            BeforeMapSet += BeforeMappingSetCollectionConvention;
            BeforeMapBag += BeforeMappingCollectionConvention;
            BeforeMapList += BeforeMappingCollectionConvention;
            BeforeMapIdBag += BeforeMappingCollectionConvention;
            BeforeMapMap += BeforeMappingCollectionConvention;
            BeforeMapComponent += DisableComponentParentAutomapping;
            BeforeMapProperty += BeforeMapPropertyConvention;
            BeforeMapManyToOne += ForeingKeyColumnConvention;
            IsComponent((t, c) => t.Namespace != "System" && !typeof(IEntity).IsAssignableFrom(t) && t.IsClass && ComponentNamespaceConvention(t));
            IsPersistentProperty((m, d) => m.MemberType != MemberTypes.Field && !_ignoredMembers.Contains(m) && !HasPersistentProperty(m.Name, m.DeclaringType));
        }

        private void BeforeMapClassConvention(IModelInspector modelinspector, Type type, IClassAttributesMapper classcustomizer)
        {
            classcustomizer.Id(idMap => idMap.Column(type.Name + PrimaryKeyColumnPostfix));
        }

        private void BeforeMapPropertyConvention(IModelInspector modelinspector, PropertyPath member, IPropertyMapper propertycustomizer)
        {
            var type = ((PropertyInfo)member.LocalMember).PropertyType;
            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                propertycustomizer.Precision(10);
                propertycustomizer.Scale(2);
            }

            if (!(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) && type.IsValueType)
                propertycustomizer.NotNullable(true);

            var memberName = member.LocalMember.Name;
            if (!memberName.EndsWith(PersistentPostfix))
                return;

            var endIndex = memberName.LastIndexOf(PersistentPostfix, StringComparison.Ordinal);
            propertycustomizer.Column(memberName.Substring(0, endIndex));
        }

        private void ForeingKeyColumnConvention(IModelInspector modelinspector, PropertyPath member, IManyToOneMapper propertycustomizer)
        {
            propertycustomizer.Column(cm => cm.Name(member.ToColumnName().Replace(member.LocalMember.GetPropertyOrFieldType().Name,
                                                                                  string.Concat(member.LocalMember.GetPropertyOrFieldType().Name, PrimaryKeyColumnPostfix))));
        }

        private void BeforeMappingSetCollectionConvention(IModelInspector modelinspector, PropertyPath member, ICollectionPropertiesMapper propertycustomizer)
        {
            propertycustomizer.Lazy(CollectionLazy.Lazy);
            propertycustomizer.Inverse(true);
            BeforeMappingCollectionConvention(modelinspector, member, propertycustomizer);
        }

        private void DisableComponentParentAutomapping(IModelInspector inspector, PropertyPath member, IComponentAttributesMapper customizer)
        {
            var parentMapping = member.LocalMember.GetPropertyOrFieldType().GetFirstPropertyOfType(member.Owner());
            DisableAutomappingFor(parentMapping);
        }

        private void DisableAutomappingFor(MemberInfo member)
        {
            if (member != null)
                _ignoredMembers.Add(member);
        }

        private void BeforeMappingCollectionConvention(IModelInspector inspector, PropertyPath member, ICollectionPropertiesMapper customizer)
        {
            if (inspector.IsManyToManyKey(member.LocalMember))
                customizer.Table(member.ManyToManyIntermediateTableName());

            customizer.Key(k => k.Column(DetermineKeyColumnName(inspector, member)));
            customizer.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
        }

        private static string DetermineKeyColumnName(IModelInspector inspector, PropertyPath member)
        {
            var otherSideProperty = member.OneToManyOtherSideProperty();
            if (inspector.IsOneToMany(member.LocalMember) && otherSideProperty != null)
                return otherSideProperty.Name + ForeignKeyColumnPostfix;

            return member.Owner().Name + ForeignKeyColumnPostfix;
        }

        private static bool ComponentNamespaceConvention(Type type)
        {
            var typeNamespace = type.Namespace != null && type.Namespace.Contains(".")
                                    ? type.Namespace.Substring(type.Namespace.LastIndexOf(".", StringComparison.Ordinal) + 1)
                                    : type.Namespace;

            return typeNamespace != null && typeNamespace.Contains(ComponentNamespace);
        }

        private static bool HasPersistentProperty(string name, Type declaringType)
        {
            var persistentPropertyName = string.Concat(name, PersistentPostfix);
            return declaringType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Any(mi => mi.Name == persistentPropertyName);
        }
    }

    public static class PropertyPathExtensions
    {
        public static Type Owner(this PropertyPath member)
        {
            return member.GetRootMember().DeclaringType;
        }

        public static Type CollectionElementType(this PropertyPath member)
        {
            return member.LocalMember.GetPropertyOrFieldType().DetermineCollectionElementOrDictionaryValueType();
        }

        public static MemberInfo OneToManyOtherSideProperty(this PropertyPath member)
        {
            return member.CollectionElementType().GetFirstPropertyOfType(member.Owner());
        }

        public static string ManyToManyIntermediateTableName(this PropertyPath member)
        {
            return string.Join(ConventionsMapper.ManyToManyIntermediateTableInfix, member.ManyToManySidesNames().OrderBy(x => x));
        }

        private static IEnumerable<string> ManyToManySidesNames(this PropertyPath member)
        {
            yield return member.Owner().Name;
            yield return member.CollectionElementType().Name;
        }
    }
}
