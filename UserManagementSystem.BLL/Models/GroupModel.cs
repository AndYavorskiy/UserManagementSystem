using System;

namespace UserManagementSystem.DAL.Entities
{
    public class GroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int UsersCount { get; set; }
    }
}
