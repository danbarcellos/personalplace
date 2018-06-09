using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Pactor.Infra.Crosscutting.Security;

namespace PersonalPlace.Domain.Base.CustomType
{
    public class EncryptedString : IUserType
    {
        private readonly IEncryptionService _encryptor;

        public EncryptedString()
        {
            _encryptor = Concealment.Encryption;
        }

        public virtual SqlType[] SqlTypes => new[] { new SqlType(DbType.String) };

        public Type ReturnedType => typeof(string);

        public bool IsMutable => false;

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var passwordString = NHibernateUtil.String.NullSafeGet(rs, names[0], session, owner);

            return passwordString != null
                ? _encryptor.Decrypt((string)passwordString)
                : null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index, session);
                return;
            }

            var hashedPassword = _encryptor.Encrypt((string)value);
            NHibernateUtil.String.NullSafeSet(cmd, hashedPassword, index, session);
        }

        public object DeepCopy(object value) => value == null ? null : string.Copy((string)value);

        public object Replace(object original, object target, object owner) => original;

        public object Assemble(object cached, object owner) => DeepCopy(cached);

        public object Disassemble(object value) => DeepCopy(value);
    }
}