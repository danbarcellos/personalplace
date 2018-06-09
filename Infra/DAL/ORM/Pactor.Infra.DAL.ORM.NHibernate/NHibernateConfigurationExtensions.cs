using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace Pactor.Infra.DAL.ORM.NHibernate
{
    public static class NHibernateConfigurationExtensions
    {
        private static readonly PropertyInfo TableMappingsProperty =
            typeof(Configuration).GetProperty("TableMappings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public static void ApplyDatabaseSchemaConventions(this Configuration configuration)
        {
            configuration.BuildMappings();
            var tables = (ICollection<Table>)TableMappingsProperty.GetValue(configuration, null);
            foreach (var table in tables)
            {
                var primaryKeyName = new StringBuilder(string.Concat("PK_", table.Name, "_"));
                primaryKeyName.Append(string.Join("_", table.PrimaryKey.ColumnIterator.Select(x => x.Name)));
                table.PrimaryKey.Name = primaryKeyName.ToString();

                foreach (var uniqueKey in table.UniqueKeyIterator)
                {
                    var uniqueKeyName = new StringBuilder(string.Concat("UQ_", table.Name, "_"));
                    uniqueKeyName.Append(string.Join("_", uniqueKey.ColumnIterator.Select(x => x.Name)));
                    uniqueKey.Name = uniqueKeyName.ToString();
                }

                foreach (var foreignKey in table.ForeignKeyIterator)
                {
                    var fkName = new StringBuilder(string.Concat("FK_", table.Name, "_"));
                    fkName.Append(foreignKey.ReferencedTable.Name);
                    fkName.Append("_");
                    fkName.Append(string.Join("_", foreignKey.ColumnIterator.Select(x => x.Name)));
                    foreignKey.Name = fkName.ToString();

                    var idx = new Index();
                    idx.AddColumns(foreignKey.ColumnIterator);
                    idx.Table = table;
                    var indexName = new StringBuilder("IX_");
                    indexName.Append(table.Name);
                    indexName.Append("_");
                    indexName.Append(string.Join("_", foreignKey.ColumnIterator.Select(x => x.Name)));
                    idx.Name = indexName.ToString();
                    table.AddIndex(idx);
                }
            }
        }
    }
}