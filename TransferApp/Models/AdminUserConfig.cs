namespace TransferApp.Models
{
    public class AdminUserConfig
    {
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }

    public class AdminUsersOptions
    {
        public List<AdminUserConfig> AdminUsers { get; set; } = new();
    }
}
