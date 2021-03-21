using RicEntityFramework.BaseRepository.Interfaces;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IAccountRepository : IEntityBaseRepository<Account>
    {
        PagedList<Account> GetAccounts(AccountResourceParameters accountResourceParameters);
    }
}
