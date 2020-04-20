using System;

namespace UserManagementSystem.BLL.Models
{
    public class GroupMemberModel
    {
        public Guid Id { get; set; }
        public string ProfileImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
