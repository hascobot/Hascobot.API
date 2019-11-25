using Hasco.Bot.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;

namespace Hasco.Bot.Infrastructure.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ChannelName { get; set; }
        public string OnJoinChannelMessage { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool isOnline { get; set; }

    }
}
