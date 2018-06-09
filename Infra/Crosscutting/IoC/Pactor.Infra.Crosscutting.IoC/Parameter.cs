using System;

namespace Pactor.Infra.Crosscutting.IoC
{
    public class Parameter
    {
        public Parameter(object value)
        {
            Value = value;
        }

        public object Value { get; protected set; }
    }

    public class TypedParameter : Parameter
    {
        public TypedParameter(Type type, object value) : base(value)
        {
            Type = type;
        }

        public Type Type { get; private set; }

        public static TypedParameter From<T>(T parameter)
        {
            return new TypedParameter(typeof (T), parameter);
        }

        public static TypedParameter From<T>(Func<T> parameterFunc)
        {
            return From(parameterFunc());
        }
    }

    public class NamedParameter : Parameter
    {
        public NamedParameter(string name, object value) : base(value)
        {
            Name = name;
        }

        public String Name { get; private set; }
    }
}