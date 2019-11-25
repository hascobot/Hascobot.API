using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Hasco.Bot.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Hasco.Bot.Core.UOW
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<BadWord> BadWords { get; set; }
    }
}
