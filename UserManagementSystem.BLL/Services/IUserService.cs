using AuthorizationService.Models;
using System;
using System.Threading.Tasks;

namespace UserManagementSystem.BLL.Services
{
    public interface IUserService
    {
        Task<DataPagedModel<UserDetailsModel>> Search(UserSearchModel searchModel);
        Task<UserDetailsModel> Get(Guid userId);
        Task<UserDetailsModel> Create(UserCreateModel userId);
        Task<UserDetailsModel> Update(UserDetailsModel userModel);
        Task Delete(Guid userId);
    }
}