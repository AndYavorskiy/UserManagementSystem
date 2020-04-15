using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagementSystem.BLL.Services;
using UserManagementSystem.DAL;

namespace UserManagementSystem.BLL
{
    public static class BLLModule
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthorizationService, Services.AuthorizationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGroupService, GroupService>();

            DALModule.Load(services, configuration);
        }
    }
}