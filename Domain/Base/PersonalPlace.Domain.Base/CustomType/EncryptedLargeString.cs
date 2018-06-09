using NHibernate.SqlTypes;

namespace PersonalPlace.Domain.Base.CustomType
{
    public class EncryptedLargeString : EncryptedString
    {
        public override SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[] { new StringClobSqlType() };
            }
        }
    }
}