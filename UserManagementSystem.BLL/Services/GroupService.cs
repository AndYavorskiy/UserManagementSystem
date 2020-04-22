using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

                query = query.Where(x => x.Name.ToUpper().Contains(searchGroup));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(x => x.UserGroups)
                .ThenInclude(x => x.User)
                .OrderBy(x => x.Name)
                .Skip(filter.PageIndex * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new GroupDetailsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    MembersCount = x.UserGroups.Where(x => x.User.IsActive).Count()
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
                .ThenInclude(x => x.User)
                .Select(x => new GroupDetailsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    MembersCount = x.UserGroups.Where(x => x.User.IsActive).Count()
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

        public Task<List<GroupCandidate>> SearchCandidates(Guid groupId, int takeFirst, string filter)
        {
            var query = dbContext.Users
                .Include(x => x.UserGroups)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var text = filter.Trim().ToUpper();

                query = query.Where(x => x.FirstName.ToUpper().Contains(text)
                    || x.LastName.ToUpper().Contains(text)
                    || x.Email.ToUpper().Contains(text));
            }

            return query
                 .OrderBy(x => x.FirstName)
                 .ThenBy(x => x.LastName)
                 .Take(takeFirst)
                 .Select(x => new GroupCandidate
                 {
                     Id = x.Id,
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     Email = x.Email,
                     ProfileImageUrl = x.ProfileImageUrl,
                     InsideGroup = x.UserGroups.Any(x => x.GroupId == groupId)
                 })
                 .ToListAsync();
        }

        public async Task<DataPagedModel<GroupMemberModel>> GetGroupMembers(Guid groupId, PagedDataRequestModel model)
        {
            var query = dbContext.UserGroups
                .Where(x => x.GroupId == groupId)
                .Include(x => x.User)
                .Where(x => x.User.IsActive);

            var totalCount = await query.CountAsync();

            var items = await query.OrderBy(x => x.User.FirstName)
                .ThenBy(x => x.User.LastName)
                .Skip(model.PageIndex * model.PageSize)
                .Take(model.PageSize)
                .Select(x => new GroupMemberModel()
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email,
                    ProfileImageUrl = x.User.ProfileImageUrl
                })
                .ToArrayAsync();

            return new DataPagedModel<GroupMemberModel>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task AddMemberToGroup(Guid groupId, Guid userId)
        {
            var isGroupAvailable = await dbContext.Groups.AnyAsync(x => x.Id == groupId);

            if (!isGroupAvailable)
            {
                throw new AppException(ExceptionType.GroupNotFound);
            }

            await dbContext.UserGroups.AddAsync(new UserGroup
            {
                GroupId = groupId,
                UserId = userId
            });

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
