using Hasco.Bot.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Core.Repositories
{
    public interface IChatUserRepository
    {
        Task<ChatUser> GetAsync(Guid id);
        Task<ChatUser> GetAsync(string displayName);
        Task<IEnumerable<ChatUser>> BrowseAsync(UserRole role);
        Task AddAsync(ChatUser user);
        Task UpdateAsync(ChatUser user);
        Task DeleteAsync(ChatUser user);

    }
}
