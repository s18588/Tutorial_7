using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Primitives;

namespace Tutorial_5and6.Handlers
{
    public class Hashish
    {
        public string CreateHash(StringValues value, string salt)
        {
            var bytes = KeyDerivation.Pbkdf2(password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(bytes);
        }

        public bool Validate(string value, string salt, string hash) => CreateHash(value, salt) == hash;

        public string CreateSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using(var gen = RandomNumberGenerator.Create())
            {
                gen.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}