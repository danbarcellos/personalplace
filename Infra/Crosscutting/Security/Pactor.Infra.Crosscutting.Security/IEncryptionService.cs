using System;
using System.IO;

namespace Pactor.Infra.Crosscutting.Security
{
    public interface IEncryptionService : IDisposable
    {
        string Encrypt(string texto);
        string Decrypt(string texto);
    }
}