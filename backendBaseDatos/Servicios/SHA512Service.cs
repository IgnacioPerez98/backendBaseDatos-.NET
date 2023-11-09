using System.Security.Cryptography;
using System.Text;

namespace backendBaseDatos.Servicios
{
    public class SHA512Service
    {
        private static string Secret = "[*t)kM%f0mF)xx(b!~]5G%S^W|ua@^=0$+LUb96FY%PH=pK%,k";

        public SHA512Service() { }

        public static string Encrypt(string entrada)
        {
            // Convert the key and entrada to byte arrays
            byte[] keyBytes = Encoding.UTF8.GetBytes(Secret);
            byte[] entradaBytes = Encoding.UTF8.GetBytes(entrada);
            string result = "";
            using (HMACSHA512 hmac = new HMACSHA512(keyBytes))
            {
                // Compute the HMAC-SHA512 hash
                byte[] hashBytes = hmac.ComputeHash(entradaBytes);

                // Convert the byte array to a hexadecimal string
                result = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            return result;
        }
    }
}
