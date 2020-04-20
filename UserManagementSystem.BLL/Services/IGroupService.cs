using System;
using System.Collections.Generic;
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
        Task<List<GroupCandidate>> SearchCandidates(Guid groupId, int takeFirst, string filter);
        Task<DataPagedModel<GroupMemberModel>> GetGroupMembers(Guid groupId, PagedDataRequestModel model);
        Task AddMemberToGroup(Guid groupId, Guid userId);
        Task DeleteMemberFromGroup(Guid groupId, Guid userId);
    }
}