using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pactor.Infra.Crosscutting.Security
{
    public class ShuffleService : IShuffleService, IDisposable
    {
        private readonly SHA512 _sha512;
        private readonly UTF8Encoding _utfEncoder;

        public ShuffleService()
        {
            _sha512 = SHA512.Create();
            _sha512.Initialize();
            _utfEncoder = new UTF8Encoding();
        }

        public string ComputeHash(string texto)
        {
            var bytes = _utfEncoder.GetBytes(texto);
            var hash = _sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public string ComputeHash(Stream stream)
        {
            var streamOriginalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                var hash = _sha512.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                stream.Position = streamOriginalPosition;
            }
        }

        public string ComputeRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
            return result;
        }

        public void Dispose()
        {
            _sha512?.Dispose();
        }
    }
}