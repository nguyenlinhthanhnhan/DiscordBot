using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IVouchUserRepository
    {
        Task<List<VouchUser>> GetAllVouchedForUser(ulong userId, Guid leagueId, bool trackChanges);
        Task<List<VouchUser>> GetListVouchedByAUserAsync(ulong userId, bool trackChanges);
        void CreateVouchedUser(VouchUser vouchedUser);
        void DeleteVouchedUser(VouchUser vouchedUser);
    }
}
