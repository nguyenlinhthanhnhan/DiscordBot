using Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryManager:IRepositoryManager
    {
        private DataContext _dataContext;
        private IUserRepository _user;
        private IVouchUserRepository _vouchedUser;
        private ILeagueRepository _league;
        private IUserLeagueVouchRepository _userLeagueVouch;

        public RepositoryManager(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IUserRepository User
        {
            get
            {
                if (_user is null) _user = new UserRepository(_dataContext);
                return _user;
            }
        }

        public ILeagueRepository League
        {
            get
            {
                if (_league is null) _league = new LeagueRepository(_dataContext);
                return _league;
            }
        }

        public IUserLeagueVouchRepository UserLeagueVouch
        {
            get
            {
                if (_userLeagueVouch is null) _userLeagueVouch = new UserLeagueVouchRepository(_dataContext);
                return _userLeagueVouch;
            }
        }

        public IVouchUserRepository VouchUser
        {
            get
            {
                if (_vouchedUser is null) _vouchedUser = new VouchUserRepository(_dataContext);
                return _vouchedUser;
            }
        }

        public Task SaveAsync() => _dataContext.SaveChangesAsync();
    }
}
