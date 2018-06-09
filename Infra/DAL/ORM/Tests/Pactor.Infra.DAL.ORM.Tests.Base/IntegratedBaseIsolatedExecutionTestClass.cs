using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using NHibernate;
using Pactor.Infra.Crosscutting.IoC.Core;
using IContainer = Pactor.Infra.Crosscutting.IoC.IContainer;

namespace Pactor.Infra.DAL.ORM.Tests.Base
{
    public abstract class IntegratedBaseIsolatedExecutionTestClass : IntegratedBaseTestClass
    {
        [ThreadStatic]
        private static readonly IDictionary<string, IContainer> TestMethodContainers;

        static IntegratedBaseIsolatedExecutionTestClass()
        {
            TestMethodContainers = new Dictionary<string, IContainer>();
        }

        protected IContainer Container
        {
            get
            {
                lock (TestMethodContainers)
                {
                    var containerKey = GetContainerKey();
                    return TestMethodContainers.ContainsKey(containerKey) ? TestMethodContainers[containerKey] : null;
                }
            }
        }

        override protected ISessionFactory SessionFactory
        {
            get { return Container.Resolve<ISessionFactory>(); }
        }

        override protected ISession Session
        {
            get { return Container.Resolve<ISession>(); }
        }

        protected override void OnSetup()
        {
            lock (TestMethodContainers)
            {
                var containerKey = GetContainerKey();
                if (TestMethodContainers.ContainsKey(containerKey))
                {
                    TestMethodContainers[containerKey].Dispose();
                    TestMethodContainers.Remove(containerKey);
                }

                TestMethodContainers.Add(containerKey, ((AutofacContainer)RootContainer).BeginLifetimeScope(IsolatedContainerRegistries));
            }
            
            IsolateTestMethodContext();
            base.OnSetup();
        }

        protected virtual void IsolatedContainerRegistries(ContainerBuilder conterBuilder) { }

        protected override void OnTeardown()
        {
            lock (TestMethodContainers)
            {
                var containerKey = GetContainerKey();
                if (TestMethodContainers.ContainsKey(containerKey))
                {
                    TestMethodContainers[containerKey].Dispose();
                    TestMethodContainers.Remove(containerKey);
                }
            }

            base.OnTeardown();
        }

        protected void IsolateTestMethodContext()
        {
            // Alterando os registros padrão do Container para o ambiente de Teste.
            // Um novo Container (LifeTimeScope) para cada método de Teste Executado.
            var builder = new ContainerBuilder();
            builder.Register(c => Container)
                   .As<IContainer>()
                   .InstancePerLifetimeScope();
            builder.Register(c => new AutofacServiceLocator(Container.GetInternalContainer()))
                   .As<IServiceLocator>()
                   .As<IServiceProvider>()
                   .InstancePerLifetimeScope();

            Container.Update(builder);
        }

        private string GetContainerKey()
        {
            return string.Concat(TestContext.FullyQualifiedTestClassName, TestContext.TestName);
        }
    }
}
