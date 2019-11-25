using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hasco.Bot.Infrastructure.Extensions.ClientUsers
{
    public class Authenticate
    {
        [Required]
        public string ChannelName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
