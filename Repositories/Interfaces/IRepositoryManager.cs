using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        ILeagueRepository League { get; }
        IUserLeagueVouchRepository UserLeagueVouch { get; }
        IVouchUserRepository VouchUser { get; }
        Task SaveAsync();
    }
}
