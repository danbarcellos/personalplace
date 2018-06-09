using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pactor.Infra.Crosscutting.Security
{
    public class EncryptionServiceRijndael : IEncryptionService
    {
        private readonly byte[] _key = { 184,  42, 196, 172, 239, 206,  14, 166, 
                                          78, 241, 178,  76, 123,  11, 228, 170,
                                         201, 246, 200,  39, 116, 194, 219, 163,
                                          42, 142, 187, 170,  44,  37,  73, 160 };

        private readonly byte[] _vector = { 217,  58, 108,  71,  11,  64, 246,  28, 
                                            130, 252,  28, 161,  99,  43, 136,  61 };

        private readonly ICryptoTransform _encryptorTransform;
        private readonly ICryptoTransform _decryptorTransform;
        private readonly UTF8Encoding _utfEncoder;

        public EncryptionServiceRijndael()
        {
            using (var rijndael = new RijndaelManaged())
            {
                _encryptorTransform = rijndael.CreateEncryptor(_key, _vector);
                _decryptorTransform = rijndael.CreateDecryptor(_key, _vector);
            }
            _utfEncoder = new UTF8Encoding();
        }
        
        public string Encrypt(string textValue)
        {
            var bytes = _utfEncoder.GetBytes(textValue);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, _encryptorTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();

                    memoryStream.Position = 0;
                    var encrypted = new byte[memoryStream.Length];
                    memoryStream.Read(encrypted, 0, encrypted.Length);

                    cryptoStream.Close();
                    memoryStream.Close();

                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public string Decrypt(string textValue)
        {
            if (string.IsNullOrWhiteSpace(textValue))
                return textValue;

            try
            {
                var bytes = Convert.FromBase64String(textValue);

                using (var memoryStream = new MemoryStream())
                {
                    using (var decryptStream = new CryptoStream(memoryStream, _decryptorTransform, CryptoStreamMode.Write))
                    {
                        decryptStream.Write(bytes, 0, bytes.Length);
                        decryptStream.FlushFinalBlock();

                        memoryStream.Position = 0;
                        var decryptedBytes = new Byte[memoryStream.Length];
                        memoryStream.Read(decryptedBytes, 0, decryptedBytes.Length);

                        decryptStream.Close();
                        memoryStream.Close();

                        return _utfEncoder.GetString(decryptedBytes);
                    }
                }
            }
            catch (Exception)
            {
                return textValue;
            }
        }

        public static byte[] GenerateEncryptionKey()
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateKey();
                return rijndael.Key;
            }
        }

        public static byte[] GenerateEncryptionVector()
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateIV();
                return rijndael.IV;
            }
        }

        public void Dispose()
        {
            _encryptorTransform.Dispose();
            _decryptorTransform.Dispose();
        }
    }
}