using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace SocialNetwork.Server.Infrastructure.Services;
public static class RefreshTokenService
{
    public static (string Raw, string Hash) Generate()
    {
        var raw = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(64));
        return (raw, Hash(raw));
    }
    public static string Hash(string raw) => 
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
}
