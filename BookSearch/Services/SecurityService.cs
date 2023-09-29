using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace BookSearch.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;

        public SecurityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string HashPassword(char[] password)
        {
            using SHA256 crypt = SHA256.Create();
            var hash = new StringBuilder();
            int iter = 2;
            var saltArr = "12qwASzx".ToCharArray();
            iter = int.Parse(_configuration["hashIterations"]!);
            saltArr = _configuration["passwordSalt"]!.ToCharArray();
            var newCharArr = new char[password.Length + saltArr.Length];
            Array.Copy(password, newCharArr, password.Length);
            Array.Copy(saltArr, 0, newCharArr, password.Length, saltArr.Length);
            byte[] crypto = Encoding.UTF8.GetBytes(newCharArr);
            for (int i = 0; i < iter; i++)
            {
                crypto = crypt.ComputeHash(crypto);
            }

            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
