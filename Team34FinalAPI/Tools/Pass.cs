using System.Security.Cryptography;
using System.Text;

namespace Team34FinalAPI.Tools
{
    public class Pass
    {
        public static string hashPassword(string Password)
        {
            using (var sha = SHA256.Create())
            {
                var asByteArray = Encoding.Default.GetBytes(Password);
                var hashedPassword = sha.ComputeHash(asByteArray);
                return Convert.ToBase64String(hashedPassword);

            }


        }
        public static void Main()
        {
            string password = "yourPassword";
            string hashedPassword = hashPassword(password);
            Console.WriteLine($"Original: {password}");
            Console.WriteLine($"Hashed: {hashedPassword}");


        }
    }
}
