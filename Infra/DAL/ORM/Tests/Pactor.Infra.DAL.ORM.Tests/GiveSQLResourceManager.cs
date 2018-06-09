using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pactor.Infra.DAL.ORM.Tests
{
    [TestClass]
    public class GiveSQLResourceManager
    {
        private const string SQLResourceText = "SELECT * FROM [Test]";

        [TestMethod]
        [TestCategory("Infra.DAL.ORM")]
        public void WhenRequireSqlScriptThenIGet()
        {
            // arrange
            var sqlResourceManager = new SqlResourceManager(Dialect.SQLServer, Assembly.GetExecutingAssembly());

            // act
            var sqlScript = sqlResourceManager.GetSqlScript("TestSql");

            // assert
            Assert.IsNotNull(sqlScript);
            Assert.AreEqual(sqlScript, SQLResourceText);
        }

        [TestMethod]
        [TestCategory("Infra.DAL.ORM")]
        public void WhenRequireSqlScriptInNewAssemblyThenIGet()
        {
            // arrange
            var sqlResourceManager = new SqlResourceManager(Dialect.SQLServer);

            // act
            var sqlScript = sqlResourceManager.GetSqlScript("TestSql");

            // assert
            Assert.IsNotNull(sqlScript);
            Assert.AreEqual(sqlScript, SQLResourceText);
        }
    }
}
