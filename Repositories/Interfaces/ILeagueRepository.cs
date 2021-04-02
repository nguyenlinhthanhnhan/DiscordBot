using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ILeagueRepository
    {
        Task<List<League>> GetLeaguesAsync(bool trackChanges);
        Task<League> GetLeagueAsync(string league, bool trackChanges);
        void CreateLeague(League league);
        void DeleteLeague(League league);
    }
}
