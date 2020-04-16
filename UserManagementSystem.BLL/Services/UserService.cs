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

        public async Task<DataPagedModel<UserDetailsModel>> Search(FilterModel searchModel)
        {
            var query = searchModel.IncludeInactive
                ? dbContext.Users.AsQueryable()
                : dbContext.Users.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(searchModel.FilterText))
            {
                var searchWords = searchModel.FilterText.Split(" ")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .ToArray();

                query = query.Where(x => searchWords.Contains(x.FirstName) || searchWords.Contains(x.LastName) || searchWords.Contains(x.Email));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(searchModel.PageIndex * searchModel.PageSize)
                .Take(searchModel.PageSize)
                .Select(x => new UserDetailsModel
                {
                    Id = x.Id,
                    Role = x.Role,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    Birthday = x.Birthday,
                    IsActive = x.IsActive
                })
                .ToArrayAsync();

            return new DataPagedModel<UserDetailsModel>
            {
                TotalCount = totalCount,
                Items = items
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
                Role = createModel.Role,
                IsActive = true
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

        private static UserDetailsModel MapUserToModel(User user) => new UserDetailsModel
        {
            Id = user.Id,
            Role = user.Role,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Birthday = user.Birthday,
            IsActive = user.IsActive
        };
    }
}
