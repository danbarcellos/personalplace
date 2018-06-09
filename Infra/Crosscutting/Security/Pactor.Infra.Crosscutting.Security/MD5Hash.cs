using System;
using System.Security.Cryptography;
using System.Text;

namespace Pactor.Infra.Crosscutting.Security
{
    public class MD5Hash
    {
        public string ComputeHash(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            using (var md5Hash = MD5.Create())
            {
                var hash = GetMd5Hash(md5Hash, input);
                return hash;
            }
        }

        public bool VerifyMd5Hash(string input, string hash)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (hash == null)
                throw new ArgumentNullException(nameof(hash));

            using (var md5Hash = MD5.Create())
            {
                // Hash the input. 
                var hashOfInput = GetMd5Hash(md5Hash, input);

                // Create a StringComparer an compare the hashes.
                var comparer = StringComparer.OrdinalIgnoreCase;

                return 0 == comparer.Compare(hashOfInput, hash);
            }
        }

        private static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }
}