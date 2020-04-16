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
    [Route("[controller]")]
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

        [HttpGet("id")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<UserDetailsModel>> Get(Guid id)
        {
            return Ok(await userService.Get(id));
        }

        [HttpGet("my-info")]
        public async Task<ActionResult<UserDetailsModel>> GetMyInfo()
        {
            return Ok(await userService.Get(User.GetLoggedInUserId()));
        }

        [HttpPost]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult<UserDetailsModel>> Create(UserCreateModel userModel)
        {
            return Ok(await userService.Create(userModel));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<UserDetailsModel>> Update(UserDetailsModel userModel)
        {
            return Ok(await userService.Update(userModel));
        }

        [HttpDelete("id")]
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await userService.Delete(id);
            return NoContent();
        }
    }
}
