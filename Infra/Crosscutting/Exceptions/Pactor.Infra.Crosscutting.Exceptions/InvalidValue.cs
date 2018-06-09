using System;
using System.Collections.Generic;

namespace Pactor.Infra.Crosscutting.Exceptions
{
    public class InvalidValue
    {
        public object Entity { get; set; }
        public Type EntityType { get; set; }
        public ICollection<object> MatchTags { get; set; }
        public string Message { get; set; }
        public string PropertyName { get; set; }
        public string PropertyPath { get; set; }
        public object RootEntity { get; set; }
        public object Value { get; set; }
    }
}