using Hasco.Bot.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Core.Repositories
{
    public interface IClientUserRepository
    {
        Task<User> GetAsync(int id);
        Task<User> GetAsync(string channelName);
        Task<IEnumerable<User>> BrowseAsyncOnlineUsers();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);

        //Interval messages
        Task<IEnumerable<IntervalMessage>> GetUserIntervalMessages(int userId);
        Task<IntervalMessage> GetIntervalMessageById(int userId, int msgId);
        Task RemoveIntervalMessage(int userId, int msgId);
        Task AddIntervalMessage(int userId, string message);

    }
}
