using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Models;
using UserManagementSystem.BLL.Services;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokenModel>> Login(AuthModel credentials)
        {
            var token = await authorizationService.Login(credentials);
            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthTokenModel>> Refresh(RefreshTokenModel refreshTokenModel)
        {
            var token = await authorizationService.Refresh(refreshTokenModel);
            return Ok(token);
        }
    }
}
