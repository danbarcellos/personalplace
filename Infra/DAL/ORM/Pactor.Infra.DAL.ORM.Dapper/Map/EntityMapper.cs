using System;
using System.Reflection;
using DapperExtensions.Mapper;

namespace Pactor.Infra.DAL.ORM.Dapper.Map
{
    public class EntityMapper<T> : ClassMapper<T> where T : class
    {
        protected PropertyMap Map(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (propertyInfo == null)
                throw new ArgumentException($"\"{propertyName}\" is not implemented or is not a property of {nameof(T)}", nameof(propertyName));
            
            return Map(propertyInfo);
        }
    }

    public enum NonPublicAccess
    {
        Field = 1,
        Protected = 2
    }
}