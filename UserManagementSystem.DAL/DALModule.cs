using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagementSystem.DAL.DbContexts;

namespace UserManagementSystem.DAL
{
    public static class DALModule
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserManagementSystemDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
