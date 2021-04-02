using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserLeagueVouchService:IUserLeagueVouchRepository
    {
        Task AddOrUpdateUserLeagueVouchAsync(ulong userId, string leagueName, ulong userVote, string reason);
        Task SaveAsync();
    }
}
