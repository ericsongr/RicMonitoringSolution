using System.Linq;
using RicEntityFramework.BaseRepository;
using RicEntityFramework.Helpers;
using RicEntityFramework.Parameters;
using RicEntityFramework.RoomRent.Interfaces;
using RicEntityFramework.RoomRent.Interfaces.IPropertyMappings;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;

namespace RicEntityFramework.RoomRent.Repositories
{
    public class RoomRepository : EntityBaseRepository<Room>, IRoomRepository
    {
        private readonly RicDbContext _context;
        private readonly IRoomPropertyMappingService _propertyMappingService;

        public RoomRepository(
            RicDbContext context
            , IRoomPropertyMappingService propertyMappingService
            ) : base(context)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public PagedList<Room> GetRooms(RoomResourceParameters roomResourceParameters)
        {
            var collectionBeforPaging =
                _context.Rooms.ApplySort(
                    roomResourceParameters.OrderBy,
                        _propertyMappingService.GetPropertyMapping<RoomDto, Room>());


            if (!string.IsNullOrEmpty(roomResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause =
                    roomResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforPaging = collectionBeforPaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }


            var roomIds = _context.Renters.Where(o => !o.IsEndRent).Select(o => o.RoomId);

            collectionBeforPaging.ToList().ForEach(room =>
            {
                room.IsOccupied = roomIds.Contains(room.Id) && !roomResourceParameters.RenterId.Equals(room.Id);
                room.Name = $"{room.Name} {(room.IsOccupied ? "(Occupied)" : "")}";
            });


            return PagedList<Room>.Create(collectionBeforPaging,
                roomResourceParameters.PageNumber,
                roomResourceParameters.PageSize);
        }
    }
}
