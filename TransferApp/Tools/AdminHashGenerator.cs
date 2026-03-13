using Microsoft.AspNetCore.Identity;
using TransferApp.Options;

namespace TransferApp.Tools
{
    public static class AdminHashGenerator
    {
        public static string Hash(string username, string password)
        {
            var hasher = new PasswordHasher<AdminUser>();
            var user = new AdminUser { Username = username };
            return hasher.HashPassword(user, password);
        }
    }
}
