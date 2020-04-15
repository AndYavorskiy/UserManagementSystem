using System;
using UserManagementSystem.DAL.Enums;

namespace AuthorizationService.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public RoleType Role { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
    }

    public class UserCreateModel : UserModel
    {
        public string Password { get; set; }
    }

    public class UserDetailsModel : UserModel
    {
        public bool IsActive { get; set; }
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
