using Hasco.Bot.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hasco.Bot.Core.Repositories
{
    public interface IBadWordRepository
    {
        Task<BadWord> GetAsync(int id);
        Task<IEnumerable<BadWord>> BrowseAsyncBadWords();
        Task AddAsync(BadWord badWord);
        Task DeleteAsync(BadWord badWord);
    }
}
