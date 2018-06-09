using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace PersonalPlace.Domain.Common.RBAC
{
    internal static class RBACInformationHelper
    {
        static ResourceManager _rm;
        private static ResourceManager ResourceManager
        {
            get { return _rm ?? (_rm = new ResourceManager("Alaris.Infra.Ortogonal.Seguranca.RBAC.RBACResources", typeof(RBACInformationHelper).Assembly)); }
        }

        internal static RBACInfo GetRBACInfo(Type type, string chavePerfil)
        {
            var field = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                            .SingleOrDefault(f => f.GetRawConstantValue().ToString() == chavePerfil);

            if (field == null)
                return null;

            return new RBACInfo(ResourceManager.GetString(ObterNomeField(type, field)),
                                      ResourceManager.GetString(string.Concat(ObterNomeField(type, field), "Desc")),
                                      chavePerfil);
        }

        internal static IEnumerable<RBACInfo> GetRBACInfo(Type type)
        {
            var informacoesRBAC = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                      .Select(f => new RBACInfo(ResourceManager.GetString(ObterNomeField(type, f)),
                                                                      ResourceManager.GetString(string.Concat(ObterNomeField(type, f), "Desc")),
                                                                      f.GetRawConstantValue().ToString()))
                                      .ToList();

            foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public))
            {
                informacoesRBAC.AddRange(GetRBACInfo(nestedType));
            }

            return informacoesRBAC;
        }

        private static string ObterNomeField(Type type, FieldInfo field)
        {
            var sbDeclaringTypeName = new StringBuilder();
            var declaringType = type.DeclaringType;
            
            while (declaringType != null)
            {
                sbDeclaringTypeName.Append(declaringType.Name);
                sbDeclaringTypeName.Append(".");
                
                declaringType = declaringType.DeclaringType;
            }

            return string.Concat(sbDeclaringTypeName.ToString(), type.Name, ".", field.Name);
        }
    }
}