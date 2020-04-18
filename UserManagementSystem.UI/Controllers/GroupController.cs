using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpDelete("id")]
        [AuthorizeRoles(Role.Admin)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await groupService.Delete(id);
            return NoContent();
        }

        [HttpPost("{groupId}/users")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult> AddMembersToGroup(Guid groupId, [FromBody] Guid[] usersIds)
        {
            await groupService.AddMembersToGroup(groupId, usersIds);
            return Ok();
        }

        [HttpDelete("{groupId}/users/{userId}")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<GroupModel>> DeleteMemberFromGroup(Guid groupId, Guid userId)
        {
            await groupService.DeleteMemberFromGroup(groupId, userId);
            return Ok();
        }
    }
}
