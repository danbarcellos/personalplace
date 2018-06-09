using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Moq;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Pactor.Infra.Crosscutting.IoC.Core;
using Pactor.Infra.Crosscutting.LogCore;
using Pactor.Infra.Crosscutting.ObjectMapper;
using Pactor.Infra.DAL.ORM.NHibernate.Facility;
using IContainer = Pactor.Infra.Crosscutting.IoC.IContainer;

namespace Pactor.Infra.DAL.ORM.Tests.Base
{
    public abstract class IntegratedBaseTestClass : BaseTestClass
    {
        private const string TestDataNotInjected = "testDataNotInjected";
        private static IConfiguration _configuration;

        [ThreadStatic]
        private static IContainer _rootContainer;

        [ThreadStatic]
        private static readonly IList<DatabaseTestParameter> DatabaseTestParameters;

        private static ContainerBuilder _containerBuilder;

        static IntegratedBaseTestClass()
        {
            DatabaseTestParameters = new List<DatabaseTestParameter>();
        }

        protected static IContainer RootContainer => _rootContainer;

        protected static IConfiguration Configuration
        {
            get
            {
                if (_configuration != null)
                    return _configuration;

                var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                return _configuration = configurationBuilder.Build();
            }
        }

        protected virtual ISessionFactory SessionFactory => RootContainer.Resolve<ISessionFactory>();

        protected virtual ISession Session => RootContainer.Resolve<ISession>();

        protected override void OnSetup()
        {
            if (!ExistVariable(TestDataNotInjected))
                Set(TestDataNotInjected, true);

            RestartDatabaseState();
            ObjectMapperBootstrap.Config(_containerBuilder, new[] { "PersonalPlace", "Pactor" });
            base.OnSetup();
        }

        protected override void OnTeardown()
        {
            TearDownNHibernateSession();
            base.OnTeardown();
        }

        protected void RestartDatabaseState()
        {
            TestConnectionProvider.CloseDatabase();
            InjectTestData();
        }

        protected void TearDownNHibernateSession()
        {
            TestConnectionProvider.CloseDatabase();
        }

        private void InjectTestData()
        {
            foreach (var databaseTestParameter in DatabaseTestParameters)
            {
                INDbUnitTest mySqlDatabase;
                if (databaseTestParameter.Dialect.Contains(".MsSql2012Dialect"))
                {
                    mySqlDatabase = new SqlDbUnitTest(databaseTestParameter.ConnectionString);
                }
                else
                {
                    throw new Exception($"Invalid NHibernate dialect: {databaseTestParameter.Dialect}");
                }

                var testClass = GetType();
                var testClassDataAttribute = (TestClassDataAttribute)testClass.GetCustomAttributes(typeof(TestClassDataAttribute)).FirstOrDefault();

                if (Get<bool>(TestDataNotInjected) || testClassDataAttribute == null || testClassDataAttribute.Transient)
                {
                    mySqlDatabase.ReadXmlSchema(databaseTestParameter.DataSetFileName);
                    mySqlDatabase.ReadXml(databaseTestParameter.DefaultDataFileName);
                    mySqlDatabase.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);

                    if (File.Exists(databaseTestParameter.DataFileName))
                    {
                        mySqlDatabase.ReadXml(databaseTestParameter.DataFileName);
                        mySqlDatabase.PerformDbOperation(DbOperationFlag.InsertIdentity);
                    }

                    if (testClassDataAttribute != null
                        && (testClassDataAttribute.Transient || Get<bool>(TestDataNotInjected))
                        && !string.IsNullOrEmpty(testClassDataAttribute.TestDataFilePath)
                        && File.Exists(testClassDataAttribute.TestDataFilePath))
                    {
                        mySqlDatabase.ReadXml(testClassDataAttribute.TestDataFilePath);
                        mySqlDatabase.PerformDbOperation(DbOperationFlag.InsertIdentity);
                    }
                    Set(TestDataNotInjected, false);
                }

                var testMethod = testClass.GetMethod(TestContext.TestName);
                var testDataFileAttribute = (TestMethodDataAttribute)testMethod.GetCustomAttributes(typeof(TestMethodDataAttribute)).FirstOrDefault();

                if (testDataFileAttribute == null || !File.Exists(testDataFileAttribute.TestDataFilePath))
                    continue;

                mySqlDatabase.ReadXml(testDataFileAttribute.TestDataFilePath);
                mySqlDatabase.PerformDbOperation(DbOperationFlag.InsertIdentity);
                Set(TestDataNotInjected, true);
            }
        }

        protected static void StartEnvironment()
        {
            // Construindo a máquina de Log.
            var logConfigFilePath = Path.GetDirectoryName(typeof(IntegratedBaseTestClass).Assembly.Location) + "\\nlog.config";
            LogBootstrap.Configure(logConfigFilePath);

            ConfigContainerBuilder();

            // Inicializando o Registry
            //Registry.Initialize(RootContainer);

            // Inicializando a máquina ORM com registros vindos dos módulos referenciados pelo assembly de teste
            StartORM();

            // Inicializando o schema do banco de dados;
            _containerBuilder.Build();
            BuildDatabaseSchema();
        }

        private static void BuildDatabaseSchema()
        {
            foreach (var configurations in RootContainer.ResolveAllMeta<Configuration>())
            {
                var databaseName = (string)configurations.Metadata["DatabaseName"];
                var cfg = configurations.Value;
                var schemaExport = new SchemaExport(cfg);
                schemaExport.Create(true, true);

                var dataSetFileName = string.Concat(@"Data\", databaseName, "DataSet.xsd");
                var defaultDataFileName = string.Concat(@"Data\", databaseName, "Data.xml");
                var dataFileName = string.Concat(databaseName, "Data.xml");

                DatabaseTestParameters.Add(new DatabaseTestParameter
                {
                    Dialect = cfg.Properties["dialect"],
                    ConnectionString = cfg.Properties["connection.connection_string"],
                    DataSetFileName = dataSetFileName,
                    DefaultDataFileName = defaultDataFileName,
                    DataFileName = dataFileName
                });
            }
        }

        private static void ConfigContainerBuilder()
        {
            _containerBuilder = new ContainerBootstrap().Config();
            _containerBuilder.RegisterBuildCallback(container => _rootContainer = new AutofacContainer(container));
        }

        protected static void StartORM()
        {
            var connectionProvider = new ConnectionProvider(Configuration);
            var ormBootstrapMock = new Mock<NHibernateORMBootstrap>(LogBootstrap.GetLogger(typeof(NHibernateORMBootstrap).FullName), connectionProvider) { CallBase = true };
            ormBootstrapMock.Object.Config(_containerBuilder);
        }

        private class DatabaseTestParameter
        {
            public string Dialect { get; set; }
            public string ConnectionString { get; set; }
            public string DataSetFileName { get; set; }
            public string DefaultDataFileName { get; set; }
            public string DataFileName { get; set; }
        }
    }
}
