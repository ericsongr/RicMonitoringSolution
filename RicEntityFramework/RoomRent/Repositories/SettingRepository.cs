using RicEntityFramework.BaseRepository;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class SettingRepository : EntityBaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(RicDbContext context) : base(context)
        {
        }
    }
}
