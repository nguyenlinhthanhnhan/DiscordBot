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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public void CreateUser(User user) => Create(user);

        public void DeleteUser(ulong id)
        {
            var user = FindByCondition(x => x.Id == id, false).SingleOrDefault();
            if(user is not null)
            {
                Delete(user);
            }
            
        }

        public async Task<User> GetUserAsync(ulong userId, bool trackChanges)
            => await FindByCondition(x => x.Id == userId, trackChanges).SingleOrDefaultAsync();

        public async Task<List<User>> GetUsersAsync(bool trackChanges)
            => await FindAll(trackChanges).ToListAsync();

        public void DetachUser(User user) => Detach(user);
    }
}
