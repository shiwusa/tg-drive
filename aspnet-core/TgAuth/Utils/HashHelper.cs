using System.Security.Cryptography;
using System.Text;

namespace TgDrive.Web.Auth;

public static class HashHelper
{
    public static string ComputeSha256HMACSignature(string token, string msg)
    {
        using SHA256 sha256Hash = SHA256.Create(); 
        byte[] secretKey = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(token));
        using var hmacSha256 = new HMACSHA256(secretKey);
        var bytes = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(msg));
        return ToHexString(bytes);
    }

    public static string ToHexString(byte[] array)
    {
        var hex = new StringBuilder(array.Length * 2);
        foreach (byte b in array)
        {
            hex.Append($"{b:x2}");
        }

        return hex.ToString();
    }
}