namespace Discussion.Core.Infrastructure.Common.Helpers;

using System.Security.Cryptography;
using System.Text;

public class CryptoHelper
{
    public static string Md5(string s)
    {
        using var provider = MD5.Create();
        var builder = new StringBuilder();

        foreach (var b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
        {
            builder.Append(b.ToString("x2").ToLower());
        }

        return builder.ToString();
    }
}