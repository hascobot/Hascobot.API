using Hasco.Bot.Core.Domain;
using Hasco.Bot.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDTO> Authenticate(string name, string password);
        Task<UserDTO> GetById(int id);
        Task<IEnumerable<UserDTO>> GetAll();
        Task<UserDTO> Create(User user, string password);
        Task Update(UserDTO user, string password = null);
        Task Delete(int id);

    }
}
