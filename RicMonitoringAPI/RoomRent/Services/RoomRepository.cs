using RicMonitoringAPI.Api.Helpers;
using RicMonitoringAPI.RoomRent.Entities;
using RicMonitoringAPI.RoomRent.Entities.Parameters;
using RicMonitoringAPI.RoomRent.Models;
using RicMonitoringAPI.RoomRent.Services.Interfaces;
using System.Linq;

namespace RicMonitoringAPI.Services.RoomRent
{
    public class RoomRepository : EntityBaseRepository<Room>, IRoomRepository
    {
        private new readonly RoomRentContext _context;
        private readonly IRoomPropertyMappingService _propertyMappingService;

        public RoomRepository(
            RoomRentContext context,
            IRoomPropertyMappingService propertyMappingService) : base(context)
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
                room.IsOccupied = roomIds.Contains(room.Id);
                room.Name = $"{room.Name} {(room.IsOccupied ? "(Occupied)" : "")}";
            });


            return PagedList<Room>.Create(collectionBeforPaging,
                roomResourceParameters.PageNumber,
                roomResourceParameters.PageSize);
        }
    }
}
