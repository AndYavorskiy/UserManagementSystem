using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Exceptions;
using UserManagementSystem.BLL.Models;
using UserManagementSystem.BLL.Utilities;
using UserManagementSystem.DAL.DbContexts;
using UserManagementSystem.DAL.Entities;

namespace UserManagementSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManagementSystemDbContext dbContext;

        public UserService(UserManagementSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DataPagedModel<UserDetailsModel>> Search(FilterModel filter)
        {
            var query = filter.IncludeInactive
                ? dbContext.Users.AsQueryable()
                : dbContext.Users.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.FilterText))
            {
                var text = filter.FilterText.Trim();

                query = query.Where(x => x.FirstName.Contains(text) || x.LastName.Contains(text) || x.Email.Contains(text));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Skip(filter.PageIndex * filter.PageSize)
                .Take(filter.PageSize)
                .ToArrayAsync();

            return new DataPagedModel<UserDetailsModel>
            {
                TotalCount = totalCount,
                Items = items.Select(MapUserToModel).ToArray()
            };
        }

        public async Task<UserDetailsModel> Get(Guid userId)
        {
            var user = (await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId))
                ?? throw new AppException(ExceptionType.UserNotFound);

            return MapUserToModel(user);
        }

        public async Task<UserDetailsModel> Create(UserCreateModel createModel)
        {
            var user = new User
            {
                FirstName = createModel.FirstName,
                LastName = createModel.LastName,
                Email = createModel.Email,
                Password = SecurePasswordHasher.Hash(createModel.Password),
                Birthday = createModel.Birthday,
                Phone = createModel.Phone,
                Gender = createModel.Gender,
                Role = createModel.Role,
                IsActive = true,
                PasswordChangeRequired = true
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return MapUserToModel(user);
        }

        public async Task<UserDetailsModel> Update(UserDetailsModel userModel)
        {
            var user = (await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userModel.Id))
              ?? throw new AppException(ExceptionType.UserNotFound);

            user.Email = userModel.Email;
            user.Role = userModel.Role;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Birthday = userModel.Birthday;
            user.Phone = userModel.Phone;
            user.IsActive = userModel.IsActive;
            user.Gender = userModel.Gender;

            dbContext.Update(user);
            await dbContext.SaveChangesAsync();

            return userModel;
        }

        public async Task Delete(Guid userId)
        {
            var user = (await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId))
                ?? throw new AppException(ExceptionType.UserNotFound);

            dbContext.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var user = (await dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == changePasswordModel.Login.ToUpper()))
                ?? throw new AppException(ExceptionType.UserNotFound);

            if (!SecurePasswordHasher.Verify(changePasswordModel.OldPassword, user.Password))
            {
                throw new AppUnauthorizedException();
            }

            user.Password = SecurePasswordHasher.Hash(changePasswordModel.NewPassword);
            user.PasswordChangeRequired = false;

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        private static UserDetailsModel MapUserToModel(User user) => new UserDetailsModel
        {
            Id = user.Id,
            Role = user.Role,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Birthday = user.Birthday,
            IsActive = user.IsActive,
            PasswordChangeRequired = user.PasswordChangeRequired,
            Gender = user.Gender,
            ProfileImageUrl = user.ProfileImageUrl
        };
    }
}
