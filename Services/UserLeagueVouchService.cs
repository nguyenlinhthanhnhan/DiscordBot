using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class UserLeagueVouchService : UserLeagueVouchRepository, IUserLeagueVouchService
    {
        private readonly IRepositoryManager _repositoryManager;
        public UserLeagueVouchService(IRepositoryManager repositoryManager, DataContext dataContext) : base(dataContext)
        {
            _repositoryManager = repositoryManager;
        }

        public Task SaveAsync() => _repositoryManager.SaveAsync();

        public async Task<Tuple<bool, uint>> AddOrUpdateUserLeagueVouchAsync(ulong userId, string leagueName, ulong userVote, string reason)
        {
            var checkRole = false;
            var leagueId = _repositoryManager.League.GetLeagueAsync(leagueName, false).GetAwaiter().GetResult().LeagueId;
            var obj = FindByCondition(x => x.UserId == userVote && x.LeagueId.Equals(leagueId),
                                      false).Include(x=>x.VouchedUsers).SingleOrDefault();

            // When certain all user in guid was inputed all to DB, delete or comment below code
            var vouchUser = await _repositoryManager.User.GetUserAsync(userId, false);
            if (vouchUser is null)
            {
                _repositoryManager.User.CreateUser(new User { Id = userId, JoinedAt = DateTime.UtcNow.ToLocalTime(), TotalVouch = 0 });
                await _repositoryManager.SaveAsync();
            }
            var user = await _repositoryManager.User.GetUserAsync(userVote, true);
            if (user is null)
            {
                _repositoryManager.User.CreateUser(new User { Id = userVote, JoinedAt = DateTime.UtcNow.ToLocalTime(), TotalVouch = 0 });
                await _repositoryManager.SaveAsync();
                user = await _repositoryManager.User.GetUserAsync(userVote, true);
            }
            // End logic

            if (obj is null)
            {
                var newUserLeagueVouch = new UserLeagueVouch()
                {
                    UserId = userVote, LeagueId = leagueId, Vouch = 1,
                    VouchedUsers = new List<VouchUser>
                    {
                        new VouchUser{Id = Guid.NewGuid(),UserVouchId = userId, Reason = reason, UserLeagueVouchLeagueId=leagueId, UserLeagueVouchUserId=userVote }
                    }
                };

                _repositoryManager.UserLeagueVouch.CreateUserLeagueVouch(newUserLeagueVouch);

                user.TotalVouch++;
                user.TotalUniqueVouch++;
                await _repositoryManager.SaveAsync();
            }
            else
            {
                var allVouchedForUser = await _repositoryManager.VouchUser.GetAllVouchedForUser(userVote, leagueId, false);
                if (allVouchedForUser.Select(x => x.UserVouchId).Contains(userId))
                {
                    user.TotalVouch++;
                    await _repositoryManager.SaveAsync();
                }
                else
                {
                    _repositoryManager.VouchUser.CreateVouchedUser(new VouchUser { Id = Guid.NewGuid(), UserVouchId = userId, Reason = reason, UserLeagueVouchLeagueId = leagueId, UserLeagueVouchUserId = userVote });
                    var userVoted = await _repositoryManager.UserLeagueVouch.UserLeagueVouchAsync(userVote, leagueId, true);
                    userVoted.Vouch++;
                    user.TotalUniqueVouch++;
                    await _repositoryManager.SaveAsync();
                    checkRole = true;
                }
            }

            return Tuple.Create(checkRole, user.TotalUniqueVouch);
        }
    }
}
