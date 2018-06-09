using System;

namespace Pactor.Infra.Crosscutting.Exceptions
{
    [Serializable]
    public class InvalidStateException : ApplicationException
    {
        private readonly InvalidValue[] _invalidValues;

        public InvalidStateException(InvalidValue[] invalidValues)
            : this(invalidValues, invalidValues[0].Entity != null ? invalidValues[0].Entity.GetType().Name : "")
        {
        }

        public InvalidStateException(InvalidValue[] invalidValues, String className)
            : base("Validation failed for: " + className)
        {
            _invalidValues = invalidValues;
        }

        public InvalidValue[] GetInvalidValues()
        {
            return _invalidValues;
        }
    }
}
