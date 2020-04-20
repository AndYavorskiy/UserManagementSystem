using System;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Models;

namespace UserManagementSystem.BLL.Services
{
    public interface IUserService
    {
        Task<DataPagedModel<UserDetailsModel>> Search(FilterModel searchModel);
        Task<UserDetailsModel> Get(Guid userId);
        Task<UserDetailsModel> Create(UserCreateModel userId);
        Task<UserDetailsModel> Update(UserDetailsModel userModel);
        Task Delete(Guid userId);
        Task ChangePassword(ChangePasswordModel changePasswordModel);
    }
}