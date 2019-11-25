using Hasco.Bot.Core.Domain;
using Hasco.Bot.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Infrastructure.Services
{
    public interface IChatUserService
    {
        Task<ChatUserDTO> GetAsync(Guid id);
        Task<ChatUserDTO> GetAsync(string displayName);
        Task<IEnumerable<ChatUserDTO>> BrowseAsync(UserRole role);
        Task AddAsync(ChatUserDTO user);
        Task UpdateAsync(ChatUserDTO user);
        Task DeleteAsync(ChatUserDTO user);
    }
}
