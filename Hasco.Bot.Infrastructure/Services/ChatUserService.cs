using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hasco.Bot.Core.Domain;
using Hasco.Bot.Core.Repositories;
using Hasco.Bot.Infrastructure.DTO;

namespace Hasco.Bot.Infrastructure.Services
{
    public class ChatUserService : IChatUserService
    {
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IMapper _mapper;
        public ChatUserService(IChatUserRepository chatUserRepository, IMapper mapper)
        {
            _chatUserRepository = chatUserRepository;
            _mapper = mapper;
        }
        public async Task<ChatUserDTO> GetAsync(Guid id)
        {
            var chatUser = await _chatUserRepository.GetAsync(id);

            return _mapper.Map<ChatUserDTO>(chatUser);
        }

        public async Task<ChatUserDTO> GetAsync(string displayName)
        {
            var chatUser = await _chatUserRepository.GetAsync(displayName);

            return _mapper.Map<ChatUserDTO>(chatUser);
        }
        public async Task<IEnumerable<ChatUserDTO>> BrowseAsync(UserRole role)
        {
            var chatUsers = await _chatUserRepository.BrowseAsync(role);

            return _mapper.Map<IEnumerable<ChatUserDTO>>(chatUsers);
        }
        public async Task AddAsync(ChatUserDTO user)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ChatUserDTO user)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteAsync(ChatUserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
