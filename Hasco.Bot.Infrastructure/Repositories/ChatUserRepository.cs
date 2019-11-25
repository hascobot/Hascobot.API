using Hasco.Bot.Core.Domain;
using Hasco.Bot.Core.Repositories;
using Hasco.Bot.Core.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Infrastructure.Repositories
{
    public class ChatUserRepository : IChatUserRepository
    {
        private readonly AppDbContext _dbContext;

        public async Task<ChatUser> GetAsync(Guid id)
            => await Task.FromResult(_dbContext.ChatUsers.SingleOrDefault(x => x.Id == id));

        public async Task<ChatUser> GetAsync(string displayName)
            => await Task.FromResult(_dbContext.ChatUsers.SingleOrDefault(x => x.DisplayName.ToLowerInvariant() == displayName.ToLowerInvariant()));

        public async Task<IEnumerable<ChatUser>> BrowseAsync(UserRole role)
        {
            var userWithRole = _dbContext.ChatUsers.AsEnumerable();

            userWithRole = userWithRole.Where(x => x.Role == role);

            return await Task.FromResult(userWithRole);
        }

        public async Task AddAsync(ChatUser user)
        {
            _dbContext.ChatUsers.Add(user);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(ChatUser user)
        {
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(ChatUser user)
        {
            _dbContext.ChatUsers.Remove(user);
            await Task.CompletedTask;
        }
    }
}
