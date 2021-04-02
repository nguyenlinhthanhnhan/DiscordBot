using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(bool trackChanges);
        Task<User> GetUserAsync(ulong userId, bool trackChanges);
        void CreateUser(User user);
        void DeleteUser(ulong id);
        void DetachUser(User user);
    }
}
