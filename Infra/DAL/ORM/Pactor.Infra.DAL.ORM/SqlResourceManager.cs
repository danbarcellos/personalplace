using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace Pactor.Infra.DAL.ORM
{
    public class SqlResourceManager : ISqlResourceManager
    {
        private static string _dialect = Dialect.SQLServer;
        private static readonly ConcurrentDictionary<Assembly, ConcurrentDictionary<string, string>> SqlResourceCache
                                = new ConcurrentDictionary<Assembly, ConcurrentDictionary<string, string>>();

        public SqlResourceManager(string dialect, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(dialect))
                throw new ArgumentException("Invalid dialect", nameof(dialect));

            _dialect = dialect;

            if (assembly != null)
                LoadCache(assembly);
        }

        public void LoadResources(Assembly assembly)
        {
            if (!SqlResourceCache.ContainsKey(assembly))
                LoadCache(assembly);
        }

        public string GetSqlScript(string resourceKey, Assembly assembly = null)
        {
            var resourceAssembly = assembly ?? Assembly.GetCallingAssembly();

            LoadResources(resourceAssembly);

            return SqlResourceCache[resourceAssembly].ContainsKey(resourceKey)
                   ? SqlResourceCache[resourceAssembly][resourceKey]
                   : null;
        }

        private static void LoadCache(Assembly assembly)
        {
            if (!SqlResourceCache.ContainsKey(assembly))
                SqlResourceCache.TryAdd(assembly, new ConcurrentDictionary<string, string>());

            var dialectPrefix = $".{_dialect}.";

            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (!resourceName.Contains(dialectPrefix) || !resourceName.EndsWith(".sql"))
                    continue;

                var sqlResourceName
                    = resourceName.Substring(resourceName.Substring(0, resourceName.LastIndexOf(".", StringComparison.Ordinal))
                                                         .LastIndexOf(".", StringComparison.Ordinal) + 1);

                sqlResourceName = sqlResourceName.Substring(0, sqlResourceName.LastIndexOf(".", StringComparison.Ordinal));

                var resource = GetSqlResource(resourceName, assembly);
                SqlResourceCache[assembly].TryAdd(sqlResourceName, resource);
            }
        }

        private static string GetSqlResource(string resourceKey, Assembly assembly)
        {
            string sqlString;
            try
            {
                using (var stream = assembly.GetManifestResourceStream(resourceKey))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        sqlString = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw new SqlResourceException($"Error on recovering SQL resource with key \"{resourceKey}\"", e);
            }

            return sqlString;
        }
    }
}