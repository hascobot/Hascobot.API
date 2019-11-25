using System;
using System.Collections.Generic;
using System.Text;

namespace Hasco.Bot.Core.Domain
{
    public class IntervalMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime LastSent { get; set; } = DateTime.UtcNow;

        public IntervalMessage(string message)
        {
            Message = message;
        }
    }
}
