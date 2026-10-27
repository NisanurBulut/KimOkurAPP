using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KimOkur.API.Helpers
{
    // Single source for the JWT signing key, used both when issuing tokens (AuthController)
    // and when validating them (Startup), so the two can never drift apart.
    public static class JwtSigningKey
    {
        public const string ConfigKey = "AppSettings:Token";

        // HMAC-SHA256 needs at least 256 bits; 64 characters keeps a comfortable margin.
        public const int MinimumLength = 64;

        public static SymmetricSecurityKey Create(IConfiguration configuration)
        {
            var token = configuration[ConfigKey];
            if (string.IsNullOrWhiteSpace(token) || token.Length < MinimumLength)
            {
                throw new InvalidOperationException(
                    $"'{ConfigKey}' must be at least {MinimumLength} characters long. " +
                    $"Set a random value with: dotnet user-secrets set \"{ConfigKey}\" \"<random value>\"");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));
        }
    }
}
