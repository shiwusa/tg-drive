using System.Security.Cryptography;
using System.Text;

namespace TgDrive.Web.Utils;

public static class HashHelper
{

    public static string ComputeSha256HMACSignature(string token, string msg)
    {
        using SHA256 sha256Hash = SHA256.Create(); 
        byte[] secretKey = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(token));
        using (var hmacsha256 = new HMACSHA256(secretKey))
        {
            var bytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(msg));
            return ToHexString(bytes);
        }
    }

    public static string ToHexString(byte[] array)
    {
        StringBuilder hex = new StringBuilder(array.Length * 2);
        foreach (byte b in array)
        {
            hex.AppendFormat("{0:x2}", b);
        }

        return hex.ToString();
    }
}