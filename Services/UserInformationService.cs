using Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserInformationService:IUserInformationService
    {
        private readonly IRepositoryManager _repositoryManager;

        public UserInformationService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        private readonly string CurrentLeague = "Ritual";

        public async Task<UserInformation> GetUserInformationAsync(ulong userId)
        {
            var user = await _repositoryManager.User.GetUserAsync(userId, false);
            var userLeagueVouch = await _repositoryManager.UserLeagueVouch.UserLeagueVouchAsync(userId, _repositoryManager.League.GetLeagueAsync(CurrentLeague, false).GetAwaiter().GetResult().LeagueId, false);

            UserInformation vouchInformation = new()
            {
                TotalVouch = user.TotalVouch,
                LeagueUniqueVouch = userLeagueVouch.Vouch,
                UniqueVouch = user.TotalUniqueVouch,
                Joined = (uint)(DateTime.UtcNow.ToLocalTime() - user.JoinedAt).Days
            };

            return vouchInformation;
        }
    }
}
