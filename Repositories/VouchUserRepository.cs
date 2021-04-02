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
    public class VouchUserRepository : RepositoryBase<VouchUser>, IVouchUserRepository
    {
        public VouchUserRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void CreateVouchedUser(VouchUser vouchedUser) => Create(vouchedUser);

        public void DeleteVouchedUser(VouchUser vouchedUser) => Delete(vouchedUser);

        public async Task<List<VouchUser>> GetAllVouchedForUser(ulong userId, Guid leagueId, bool trackChanges)
            => await FindByCondition(x => x.UserLeagueVouchUserId == userId && x.UserLeagueVouchLeagueId == leagueId,
                                     trackChanges).ToListAsync();

        public async Task<List<VouchUser>> GetListVouchedByAUserAsync(ulong userId, bool trackChanges)
            => await FindByCondition(x => x.UserVouchId == userId, trackChanges).ToListAsync();
    }
}
