using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Models;
using UserManagementSystem.DAL.Enums;

namespace UserManagementSystem.BLL.Services
{
    public interface IUserService
    {
        Task<DataPagedModel<UserDetailsModel>> Search(FilterModel searchModel);
        Task<UserDetailsModel> Get(Guid userId, Guid currentUserId, RoleType currentUserRole);
        Task<UserDetailsModel> Create(UserCreateModel userId, RoleType currentUserRole);
        Task<UserDetailsModel> Update(UserDetailsModel userModel, Guid currentUserId, RoleType currentUserRole);
        Task ChangePassword(ChangePasswordModel changePasswordModel);
        Task<List<GroupModel>> GetUserGroups(Guid userId, Guid currentUserId, RoleType currentUserRole);
    }
}