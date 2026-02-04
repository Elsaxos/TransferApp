namespace TransferApp.Security
{
    public class AdminUsersOptions
    {
        public List<AdminUser> Users { get; set; } = new();
    }

    public class AdminUser
    {
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }
}
