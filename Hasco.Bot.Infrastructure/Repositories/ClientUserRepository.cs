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
    public class ClientUserRepository : IClientUserRepository
    {
        private AppDbContext _dbContext;

        public ClientUserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetAsync(int id)
            => await Task.FromResult(_dbContext.Users.SingleOrDefault(x => x.Id == id));

        public async Task<User> GetAsync(string channelName)
            => await Task.FromResult(_dbContext.Users.SingleOrDefault(x => x.ChannelName == channelName));


        public async Task<IEnumerable<User>> BrowseAsyncOnlineUsers()
        {
            var onlineUsers = _dbContext.Users.AsEnumerable();

            onlineUsers = onlineUsers.Where(x => x.isOnline == false);

            return await Task.FromResult(onlineUsers);
        }

        public async Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            await Task.CompletedTask;
        }
        public async Task UpdateAsync(User user)
        {
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<IntervalMessage>> GetUserIntervalMessages(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            var userIntervalMessages = user.IntervalMessages;

            return userIntervalMessages;
        }
        public async Task<IntervalMessage> GetIntervalMessageById(int userId, int msgId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            var userIntervalMessage = user.IntervalMessages.SingleOrDefault(x => x.Id == msgId);

            return userIntervalMessage;
        }
        public async Task RemoveIntervalMessage(int userId, int msgId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var userIntervalMessage = user.IntervalMessages.SingleOrDefault(x => x.Id == msgId);

            user.IntervalMessages.Remove(userIntervalMessage);

            await Task.CompletedTask;
        }
        public async Task AddIntervalMessage(int userId, string message)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            var newIntervalMessage = new IntervalMessage(message);
            user.IntervalMessages.Add(newIntervalMessage);
        }
    }
}
