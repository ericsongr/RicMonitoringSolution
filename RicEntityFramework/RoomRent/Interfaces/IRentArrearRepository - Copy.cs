
using System.Collections.Generic;
using System.Linq;
using RicEntityFramework.BaseRepository.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Interfaces
{
    public interface IRenterCommunicationRepository : IEntityBaseRepository<RenterCommunicationHistory>
    {
        IList<RenterCommunicationHistory> Find(int id);

        IQueryable<RenterCommunicationHistory> FindAll();

        long Save(RenterCommunicationHistory memberComm);
    }
}
