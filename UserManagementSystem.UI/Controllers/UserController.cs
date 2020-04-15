using AuthorizationService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Services;

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
        public async Task<ActionResult<DataPagedModel<UserDetailsModel>>> Search([FromQuery] UserSearchModel searchModel)
        {
            return Ok(await userService.Search(searchModel));
        }

        [HttpGet("id")]
        public async Task<ActionResult<UserDetailsModel>> Get(Guid id)
        {
            return Ok(await userService.Get(id));
        }

        [HttpPost]
        public async Task<ActionResult<UserDetailsModel>> Create(UserCreateModel userModel)
        {
            return Ok(await userService.Create(userModel));
        }

        [HttpPut]
        public async Task<ActionResult<UserDetailsModel>> Update(UserDetailsModel userModel)
        {
            return Ok(await userService.Update(userModel));
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await userService.Delete(id);
            return NoContent();
        }
    }
}
