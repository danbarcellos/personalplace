using System;
using System.Collections.ObjectModel;

namespace PersonalPlace.Domain.Base
{
    public interface IUserContext
    {
        Guid Id { get; }
        string Login { get; }
        string Name { get; }
        ReadOnlyDictionary<string, string> Properties { get; }
        bool IsInRole(string role);
    }
}