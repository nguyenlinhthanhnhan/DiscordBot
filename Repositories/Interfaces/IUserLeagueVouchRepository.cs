using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserLeagueVouchRepository
    {
        Task<List<UserLeagueVouch>> GetUserLeagueVouchesAsync(bool trackChanges);
        Task<UserLeagueVouch> UserLeagueVouchAsync(ulong userId, Guid leagueId, bool trackChanges);
        void CreateUserLeagueVouch(UserLeagueVouch userLeagueVouch);
        void DeleteUserLeagueVouch(UserLeagueVouch userLeagueVouch);
    }
}
