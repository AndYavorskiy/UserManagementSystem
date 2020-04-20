using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Constants;
using UserManagementSystem.BLL.Models;
using UserManagementSystem.BLL.Services;
using UserManagementSystem.UI.Attributes;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }


        [HttpGet("search")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<DataPagedModel<GroupDetailsModel>>> Search([FromQuery] FilterModel filter)
        {
            return Ok(await groupService.Search(filter));
        }

        [HttpGet("{id}")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<GroupDetailsModel>> Get(Guid id)
        {
            return Ok(await groupService.Get(id));
        }

        [HttpPost]
        [AuthorizeRoles(Role.Admin)]
        public async Task<ActionResult<GroupModel>> Create(GroupModel userModel)
        {
            return Ok(await groupService.Create(userModel));
        }

        [HttpPut]
        [AuthorizeRoles(Role.Admin)]
        public async Task<ActionResult<GroupModel>> Update(GroupModel userModel)
        {
            return Ok(await groupService.Update(userModel));
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(Role.Admin)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await groupService.Delete(id);
            return NoContent();
        }

        [HttpGet("{groupId}/candidates")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<List<GroupCandidate>>> SearchCandidates(Guid groupId, int takeFirst, string filter)
        {
            return Ok(await groupService.SearchCandidates(groupId, takeFirst, filter));
        }

        [HttpGet("{groupId}/members")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<DataPagedModel<GroupMemberModel>>> GetGroupMembers(Guid groupId, [FromQuery]PagedDataRequestModel model)
        {
            return Ok(await groupService.GetGroupMembers(groupId, model));
        }

        [HttpPost("{groupId}/members")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult> AddMemberToGroup(Guid groupId, [FromBody] AddGroupMemberModel model)
        {
            await groupService.AddMemberToGroup(groupId, model.UserId);
            return Ok();
        }

        [HttpDelete("{groupId}/members/{userId}")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<GroupModel>> DeleteMemberFromGroup(Guid groupId, Guid userId)
        {
            await groupService.DeleteMemberFromGroup(groupId, userId);
            return Ok();
        }
    }
}
