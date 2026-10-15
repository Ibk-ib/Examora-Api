
using System.Security.Cryptography;

namespace UniversityAcademicApi.Services
{
    public class PasswordHashingService : IPasswordHashingService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 600_000;

        public string HashPassword(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"PBKDF2-SHA256${Iterations}$" +
                   $"{Convert.ToBase64String(salt)}$" +
                   $"{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            string[] parts = passwordHash.Split('$');

            if (parts.Length != 4 ||
                parts[0] != "PBKDF2-SHA256" ||
                !int.TryParse(parts[1], out int iterations) ||
                iterations < 600_000 ||
                iterations > 1_000_000)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] storedHash = Convert.FromBase64String(parts[3]);

                if (salt.Length != SaltSize ||
                    storedHash.Length != HashSize)
                {
                    return false;
                }

                byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    HashSize);

                return CryptographicOperations.FixedTimeEquals(
                    computedHash,
                    storedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
