using Hasco.Bot.Core.Domain;
using Hasco.Bot.Core.Repositories;
using Hasco.Bot.Core.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Infrastructure.Repositories
{
    public class BadWordRepository : IBadWordRepository
    {

        private AppDbContext _dbContext;

        public BadWordRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BadWord> GetAsync(int id)
            => await Task.FromResult(_dbContext.BadWords.SingleOrDefault(x => x.Id == id));

        public async Task AddAsync(BadWord badWord)
        {
            _dbContext.BadWords.Add(badWord);
            _dbContext.SaveChanges();

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BadWord>> BrowseAsyncBadWords()
        {
            var badWords = _dbContext.BadWords.AsEnumerable();

            return await Task.FromResult(badWords);
        }

        public async Task DeleteAsync(BadWord badWord)
        {
            _dbContext.BadWords.Remove(badWord);

            await Task.CompletedTask;
        }
    }
}
