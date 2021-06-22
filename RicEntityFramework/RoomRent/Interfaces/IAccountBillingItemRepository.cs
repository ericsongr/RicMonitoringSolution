
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IAccountBillingItemRepository : IEntityBaseRepository<AccountBillingItem>
    {
        void AddSmsFee(int accountId, int number, int totalSmsBill);
    }
}
