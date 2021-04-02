using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LeagueRepository : RepositoryBase<League>, ILeagueRepository
    {
        public LeagueRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void CreateLeague(League league)
        {
            Create(league);
            _dataContext.SaveChanges();
        }

        public void DeleteLeague(League league) => Delete(league);

        public async Task<League> GetLeagueAsync(string league, bool trackChanges)
            => await FindByCondition(x => x.LeagueName.Equals(league), trackChanges).SingleOrDefaultAsync();

        public async Task<List<League>> GetLeaguesAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();
    }
}
