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
    public class UserLeagueVouchRepository : RepositoryBase<UserLeagueVouch>, IUserLeagueVouchRepository
    {
        public UserLeagueVouchRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void CreateUserLeagueVouch(UserLeagueVouch userLeagueVouch) => Create(userLeagueVouch);
        public void DeleteUserLeagueVouch(UserLeagueVouch userLeagueVouch) => Delete(userLeagueVouch);

        public async Task<List<UserLeagueVouch>> GetUserLeagueVouchesAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();

        public async Task<UserLeagueVouch> UserLeagueVouchAsync(ulong userId, Guid leagueId, bool trackChanges)
            => await FindByCondition(x => x.UserId == userId && x.LeagueId == leagueId,
                                     trackChanges).SingleOrDefaultAsync();

        public async Task<List<UserLeagueVouch>> GetUserVouchesAsync(ulong userId, bool trackChanges)
            => await FindByCondition(x => x.UserId == userId, trackChanges).ToListAsync();
    }
}
