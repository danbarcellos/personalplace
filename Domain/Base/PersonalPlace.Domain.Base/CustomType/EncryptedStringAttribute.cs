using System;

namespace PersonalPlace.Domain.Base.CustomType
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EncryptedStringAttribute : Attribute
    {
    }
}