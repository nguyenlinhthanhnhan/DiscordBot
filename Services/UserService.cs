using Models;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : UserRepository, IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        public UserService(DataContext dataContext, IRepositoryManager repositoryManager) : base(dataContext)
        {
            _repositoryManager = repositoryManager;
        }

        public Task SaveAsync() => _repositoryManager.SaveAsync();
    }
}
