using System.Threading.Tasks;
using UserManagementSystem.BLL.Models;

namespace UserManagementSystem.BLL.Services
{
    public interface IAuthorizationService
    {
        Task<AuthTokenModel> Login(AuthModel credentials);
        Task<AuthTokenModel> Refresh(RefreshTokenModel refreshTokenModel);
    }
}