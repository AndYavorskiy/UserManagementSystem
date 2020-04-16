using System;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Models;

namespace UserManagementSystem.BLL.Services
{
    public interface IGroupService
    {
        Task<DataPagedModel<GroupDetailsModel>> Search(FilterModel filter);
        Task<GroupDetailsModel> Get(Guid groupId);
        Task<GroupModel> Create(GroupModel createModel);
        Task<GroupModel> Update(GroupModel groupModel);
        Task Delete(Guid groupId);
        Task AddMembersToGroup(Guid groupId, Guid[] usersIds);
        Task DeleteMemberFromGroup(Guid groupId, Guid userId);
    }
}