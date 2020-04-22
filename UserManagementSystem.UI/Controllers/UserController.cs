using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Constants;
using UserManagementSystem.BLL.Extensions;
using UserManagementSystem.BLL.Models;
using UserManagementSystem.BLL.Services;
using UserManagementSystem.UI.Attributes;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("search")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<DataPagedModel<UserDetailsModel>>> Search([FromQuery] FilterModel filter)
        {
            return Ok(await userService.Search(filter));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDetailsModel>> Get(Guid id)
        {
            var currentUserId = User.GetCurrentUserId();
            var currentUserRole = User.GetCurrentUserRole();

            return Ok(await userService.Get(id, currentUserId, currentUserRole));
        }

        [HttpGet("my-info")]
        public Task<ActionResult<UserDetailsModel>> GetMyInfo()
        {
            return Get(User.GetCurrentUserId());
        }

        [HttpPost]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<UserDetailsModel>> Create(UserCreateModel userModel)
        {
            return Ok(await userService.Create(userModel, User.GetCurrentUserRole()));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<UserDetailsModel>> Update(UserDetailsModel userModel)
        {
            var currentUserId = User.GetCurrentUserId();
            var currentUserRole = User.GetCurrentUserRole();

            return Ok(await userService.Update(userModel, currentUserId, currentUserRole));
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            await userService.ChangePassword(changePasswordModel);
            return NoContent();
        }

        [HttpGet("{id}/groups")]
        [Authorize]
        public async Task<ActionResult<GroupModel>> GetUserGroups(Guid id)
        {
            var currentUserId = User.GetCurrentUserId();
            var currentUserRole = User.GetCurrentUserRole();

            return Ok(await userService.GetUserGroups(id, currentUserId, currentUserRole));
        }
    }
}
