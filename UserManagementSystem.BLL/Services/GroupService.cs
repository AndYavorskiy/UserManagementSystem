using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Exceptions;
using UserManagementSystem.BLL.Models;
using UserManagementSystem.DAL.DbContexts;
using UserManagementSystem.DAL.Entities;

namespace UserManagementSystem.BLL.Services
{
    public class GroupService : IGroupService
    {
        private readonly UserManagementSystemDbContext dbContext;

        public GroupService(UserManagementSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DataPagedModel<GroupDetailsModel>> Search(FilterModel filter)
        {
            var query = filter.IncludeInactive
                ? dbContext.Groups.AsQueryable()
                : dbContext.Groups.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.FilterText))
            {
                var searchGroup = filter.FilterText.Trim().ToUpper();

                query = query.Where(x => x.Name.ToUpper() == searchGroup);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(x => x.UserGroups)
                .Skip(filter.PageIndex * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new GroupDetailsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    MembersCount = x.UserGroups.Count()
                })
                .ToArrayAsync();

            return new DataPagedModel<GroupDetailsModel>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<GroupDetailsModel> Get(Guid groupId)
        {
            return (await dbContext.Groups
                .Include(x => x.UserGroups)
                .Select(x => new GroupDetailsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    MembersCount = x.UserGroups.Count()
                })
                .FirstOrDefaultAsync(x => x.Id == groupId)) ?? throw new AppException(ExceptionType.GroupNotFound);
        }

        public async Task<GroupModel> Create(GroupModel createModel)
        {
            var group = new Group
            {
                Name = createModel.Name,
                IsActive = true
            };

            await dbContext.Groups.AddAsync(group);
            await dbContext.SaveChangesAsync();

            return new GroupModel
            {
                Id = group.Id,
                Name = group.Name,
                IsActive = group.IsActive
            };
        }

        public async Task<GroupModel> Update(GroupModel groupModel)
        {
            var group = (await dbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupModel.Id))
                ?? throw new AppException(ExceptionType.GroupNotFound);

            group.Name = groupModel.Name;
            group.IsActive = groupModel.IsActive;

            dbContext.Update(group);
            await dbContext.SaveChangesAsync();

            return groupModel;
        }

        public async Task Delete(Guid groupId)
        {
            var group = (await dbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId))
                ?? throw new AppException(ExceptionType.GroupNotFound);

            dbContext.Remove(group);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddMembersToGroup(Guid groupId, Guid[] usersIds)
        {
            var isGroupAvailable = await dbContext.Groups.AnyAsync(x => x.Id == groupId);

            if (!isGroupAvailable)
            {
                throw new AppException(ExceptionType.GroupNotFound);
            }

            var distinctUsersIds = usersIds
                .Distinct()
                .Select(userId => new UserGroup
                {
                    GroupId = groupId,
                    UserId = userId
                })
                .ToArray();

            await dbContext.UserGroups.AddRangeAsync(distinctUsersIds);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteMemberFromGroup(Guid groupId, Guid userId)
        {
            var userGroup = (await dbContext.UserGroups.FirstOrDefaultAsync(x => x.GroupId == groupId && x.UserId == userId))
                ?? throw new AppException(ExceptionType.InvalidOperation);

            dbContext.UserGroups.Remove(userGroup);
            await dbContext.SaveChangesAsync();
        }
    }
}
