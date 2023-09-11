using System.Security.Cryptography;
using System.Text;

namespace Metafar.Challange.Common.Helpers
{
    public static class CryptographyHelper
    {
        public static string ComputeHashSha256(string secret)
            => Base64Helper.Encode(ComputeHashSha256(Encoding.UTF8.GetBytes(secret)));

        private static byte[] ComputeHashSha256(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(toBeHashed);
            }
        }
    }
}
