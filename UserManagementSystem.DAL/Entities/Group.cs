using System;
using System.Collections.Generic;

namespace UserManagementSystem.DAL.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
    }
}
