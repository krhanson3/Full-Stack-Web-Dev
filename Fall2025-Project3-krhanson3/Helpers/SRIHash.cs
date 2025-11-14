using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Fall2025_Project3_krhanson3.Helpers
{
    public static class SRIHelper
    {
        public static string ComputeSRI(byte[] data)
        {
            using var sha384 = SHA384.Create();
            var hash = sha384.ComputeHash(data);
            return "sha384-" + Convert.ToBase64String(hash);
        }

        public static async Task<string> ComputeSRIFromUrlAsync(string url)
        {
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(url);
            return ComputeSRI(bytes);
        }
    }
}
