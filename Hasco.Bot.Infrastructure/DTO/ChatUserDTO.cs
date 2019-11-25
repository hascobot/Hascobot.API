using Hasco.Bot.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hasco.Bot.Infrastructure.DTO
{
    public class ChatUserDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public UserRole? Role { get; set; }
    }
}
