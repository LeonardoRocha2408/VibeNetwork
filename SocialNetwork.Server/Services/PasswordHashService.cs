using Microsoft.AspNetCore.Identity;
using SocialNetwork.Server.Scaffolds;

namespace SocialNetwork.Server.Infrastructure.Services
{
    public class PasswordHashService(PasswordHasher<UserScaffold> passwordHasher)
    {
        private PasswordHasher<UserScaffold> _passwordHasher = passwordHasher;
        // Hashing password 
        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }
        // Verify hash password
        public PasswordVerificationResult VerifyPassword(string passwordHash, string password)
        {
            return _passwordHasher.VerifyHashedPassword(null!, passwordHash, password);
        }
    }
}
