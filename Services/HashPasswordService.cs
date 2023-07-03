using OTPApplication.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace OTPApplication.Services
{
    public class HashPasswordService : IHashPasswordService
    {
        /// <summary>
        /// Encrypting the password based of user Id and date time.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>A string composed of 6 random characters of the hased password</returns>
        public async Task<string> HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = await Task.Run(() => sha256.ComputeHash(passwordBytes));
                string hashedPassword = Convert.ToBase64String(hashedBytes);

                // Make the hased password upper case
                hashedPassword = hashedPassword.ToUpper();

                // Get only 6 random values from the hashed password
                string randomNumbers = new string(hashedPassword.ToArray());
                if (randomNumbers.Length > 6)
                {
                    randomNumbers = randomNumbers.Substring(0, 6);
                }

                return randomNumbers;
            }
        }
    }
}
