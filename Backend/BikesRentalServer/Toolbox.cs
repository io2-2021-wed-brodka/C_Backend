using System.Security.Cryptography;
using System.Text;

namespace BikesRentalServer
{
    public static class Toolbox
    {
        public static string ComputeHash(string data)
        {
            using var sha = SHA256.Create();
            
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
            var sb = new StringBuilder();
            foreach (var dataByte in bytes)
                sb.Append(dataByte.ToString("x2"));

            return sb.ToString();
        }
    }
}
