using System;

namespace Pactor.Infra.DAL.ORM
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SealedOperationOptionAttribute : Attribute
    {
        private readonly Type _expectedException;
        private readonly int _attempts;
        private readonly bool _merge;

        public SealedOperationOptionAttribute(Type expectedException, int attempts = 1, bool merge = true)
        {
            _expectedException = expectedException;
            _attempts = attempts;
            _merge = merge;
        }

        public Type ExpectedException { get { return _expectedException; } }

        public int Attempts { get { return _attempts; } }

        public bool MergeResult { get { return _merge; } }
    }
}