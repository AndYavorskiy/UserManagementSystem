using AuthorizationService.Models;
using System.Threading.Tasks;

namespace UserManagementSystem.BLL.Services
{
    public interface IAuthorizationService
    {
        Task<AuthTokenModel> Login(AuthModel credentials);
        Task<AuthTokenModel> Refresh(RefreshTokenModel refreshTokenModel);
    }
}