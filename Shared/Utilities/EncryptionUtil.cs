using System.Security.Cryptography;
using System.Text;
using Shared.Exceptions;

namespace Shared.Utilities
{
    public static class EncryptionUtil
    {
      
        public static string GenerateSha512Hash(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentIsNullException($"{nameof(inputString)} should not be empty when generating SHA512 hash.");
            }
            
            using var sha512Hash = SHA512.Create();

            //From string to byte array
            var sourceBytes = Encoding.UTF8.GetBytes(inputString);
            var hashBytes = sha512Hash.ComputeHash(sourceBytes);
            var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            return hash;
        }
    }
}
