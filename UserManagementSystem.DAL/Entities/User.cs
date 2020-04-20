using System;
using System.Collections.Generic;
using UserManagementSystem.DAL.Enums;

namespace UserManagementSystem.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public RoleType Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
        public bool PasswordChangeRequired { get; set; }
        public string ProfileImageUrl { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
    }
}
