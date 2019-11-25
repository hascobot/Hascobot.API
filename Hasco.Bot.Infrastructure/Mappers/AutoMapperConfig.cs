using AutoMapper;
using Hasco.Bot.Core.Domain;
using Hasco.Bot.Infrastructure.DTO;
using Hasco.Bot.Infrastructure.Extensions.ClientUsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hasco.Bot.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChatUser, ChatUserDTO>();
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<Register, User>();
                cfg.CreateMap<Update, User>();
            })
            .CreateMapper();
    }
}
