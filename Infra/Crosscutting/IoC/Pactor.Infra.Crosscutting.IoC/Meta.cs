using System.Collections.Generic;

namespace Pactor.Infra.Crosscutting.IoC
{
    public class Meta<T>
    {
        public Meta(T value, IDictionary<string, object> metadata)
        {
            Value = value;
            Metadata = metadata;
        }

        public IDictionary<string, object> Metadata { get; private set; }

        public T Value { get; private set; } 
    }
}