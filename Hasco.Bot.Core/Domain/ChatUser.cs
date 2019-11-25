using System;
using System.Collections.Generic;
using System.Text;

namespace Hasco.Bot.Core.Domain
{
    public class ChatUser : Entity
    {
        public string UserId { get; protected set; }
        public string DisplayName { get; protected set; }
        public UserRole? Role { get; protected set; }
        public int Tokens { get; protected set; }

    }
}
