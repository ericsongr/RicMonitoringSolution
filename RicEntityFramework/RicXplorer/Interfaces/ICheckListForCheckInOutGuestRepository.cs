using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.Interfaces 
{
    public interface ICheckListForCheckInOutGuestRepository : IEntityBaseRepository<CheckListForCheckInOutGuest>
    {
        IQueryable<CheckListForCheckInOutGuest> FindAll();
    }
}
