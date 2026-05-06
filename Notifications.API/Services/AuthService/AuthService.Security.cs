using System.Security.Cryptography;
using System.Text;

namespace Notifications.API.Service.AuthService;

public partial class AuthService
{
    private string GenerateKey(int length = 32)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var result = new StringBuilder(length);
        var buffer = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);

        for (int i = 0; i < length; i++)
        {
            var index = buffer[i] % chars.Length;
            result.Append(chars[index]);
        }

        return result.ToString();
    }
}