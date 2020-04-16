using System;
using UserManagementSystem.DAL.Enums;

namespace UserManagementSystem.BLL.Models
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
}
