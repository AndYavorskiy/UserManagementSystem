using System;

namespace UserManagementSystem.BLL.Models
{
    public class GroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
