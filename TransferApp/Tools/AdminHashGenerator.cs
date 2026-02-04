using Microsoft.AspNetCore.Identity;
using TransferApp.Models;

namespace TransferApp.Tools
{
    public static class AdminHashGenerator
    {
        public static string Hash(string username, string password)
        {
            var hasher = new PasswordHasher<AdminUserConfig>();
            var user = new AdminUserConfig { Username = username };
            return hasher.HashPassword(user, password);
        }
    }
}
