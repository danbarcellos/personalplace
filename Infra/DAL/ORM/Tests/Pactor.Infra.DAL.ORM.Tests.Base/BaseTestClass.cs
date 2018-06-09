using System;
using System.Collections.Generic;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pactor.Infra.DAL.ORM.Tests.Base
{
    public abstract class BaseTestClass
    {
        private TestContext _mTestContext;
        private static readonly Dictionary<string, Dictionary<string, object>> StaticVariables = new Dictionary<string, Dictionary<string, object>>();

        protected virtual void OnSetup() { }

        protected virtual void OnTeardown() { }

        public TestContext TestContext
        {
            get => _mTestContext;
            set => _mTestContext = value;
        }

        [TestInitialize]
        public void Setup()
        {
            NHibernateProfiler.Initialize();
            OnSetup();
        }

        [TestCleanup]
        public void Teardown()
        {
            OnTeardown();
        }

        protected bool ExistVariable(string key)
        {
            var className = GetType().FullName;
            return StaticVariables.ContainsKey(className) && StaticVariables[className].ContainsKey(key);
        }

        protected void Set(string key, object value)
        {
            var className = GetType().FullName;
            if (!StaticVariables.ContainsKey(className))
            {
                StaticVariables.Add(className, new Dictionary<string, object>());
            }

            if (StaticVariables[className].ContainsKey(key))
            {
                StaticVariables[className][key] = value;
            }
            else
            {
                StaticVariables[className].Add(key, value);
            }
        }

        protected TReturn Get<TReturn>(string key)
        {
            var className = GetType().FullName;

            if (!StaticVariables.ContainsKey(className) || !StaticVariables[className].ContainsKey(key))
                throw new ArgumentOutOfRangeException(nameof(key));

            return (TReturn)StaticVariables[className][key];
        }
    }
}
