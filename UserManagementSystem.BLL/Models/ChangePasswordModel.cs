namespace UserManagementSystem.BLL.Models
{
    public class ChangePasswordModel
    {
        public string Login { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
