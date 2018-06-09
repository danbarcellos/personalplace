using System;

namespace Pactor.Infra.DAL.ORM.Tests.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassDataAttribute : Attribute
    {
        public TestClassDataAttribute(string testDataFilePath = null, bool transient = true)
        {
            TestDataFilePath = testDataFilePath;
            Transient = transient;
        }

        public string TestDataFilePath { get; set; }

        public bool Transient { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodDataAttribute : Attribute
    {
        public TestMethodDataAttribute(string testDataFilePath)
        {
            TestDataFilePath = testDataFilePath;
        }

        public string TestDataFilePath { get; set; }
    }
}